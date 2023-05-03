using Mediator;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.PipelineBehaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IMessage
    where TResponse : notnull
    {

        private const string DefaultRequestLogFormat =
        "[RequestInfo {Method} {Path} {QueryString} {Headers} {Host} {Body}]";

        private const string DefaultResponseLogFormat = "[RepsosneInfo {Status} {@Body}]";

        private readonly HttpContext _httpContext;

        public LoggingBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public async ValueTask<TResponse> Handle(
            TRequest message,
            CancellationToken cancellationToken,
            MessageHandlerDelegate<TRequest, TResponse> next)
        {
            const string prefix = nameof(LoggingBehavior<TRequest, TResponse>);

            using Activity? activity = GetActivityContext();
            using (LogContext.PushProperty(Shared.Libraries.Constants.Serilog.TraceId, GetTraceId()))
            {
                try
                {
                    await LoggingRequest(prefix, _httpContext);

                    Stopwatch timer = new();
                    timer.Start();
                    TResponse response = await next(message, cancellationToken);
                    timer.Stop();
                    TimeSpan timeTaken = timer.Elapsed;
                    if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
                    {
                        Log.Warning("[{Perf-Possible}] The request {X-RequestData} took {TimeTaken} seconds.",
                            prefix, typeof(TRequest).Name, timeTaken.Seconds);
                    }

                    await LoggingResponse(prefix, response);

                    return response;
                }
                catch (Exception exception)
                {
                    Log.Error("{@Exception}", exception);
                    await LoggingResponse(prefix, exception, ActivityStatusCode.Error);

                    throw new UnreachableException(exception.Message, exception);
                }
                finally
                {
                    SetResponseTraceId(_httpContext.Response.Headers);
                }
            }
        }

        private static async Task LoggingResponse(string prefix, object response,
            ActivityStatusCode statusCode = ActivityStatusCode.Ok)
        {
            Log.Information("[{Prefix}] Handled {X-RequestData}", prefix, typeof(TRequest).FullName);
            Log.Information(DefaultResponseLogFormat, statusCode, response);

            using Activity? activity = GetActivityContext();
            activity?.SetTag("Response", JsonConvert.SerializeObject(response));
            activity?.SetTag(nameof(ActivityStatusCode), statusCode);
            activity?.SetStatus(statusCode);
        }

        private static void SetResponseTraceId(IHeaderDictionary headers)
        {
            headers.TryAdd(Shared.Libraries.Constants.Serilog.TraceId, GetTraceId());
        }


        private static async Task LoggingRequest(string prefix, HttpContext httpContext)
        {
            Log.Information("[{Prefix}] Handle request={X-RequestData} and response={X-ResponseData}",
                prefix, typeof(TRequest).Name, typeof(TResponse).FullName);

            Log.Information(
                DefaultRequestLogFormat,
                httpContext.Request.Method, httpContext.Request.Path,
                httpContext.Request.QueryString,
                FormatHeaders(httpContext.Request.Headers),
                httpContext.Request.Host,
                await ReadBodyFromRequest(httpContext.Request));

            using Activity? activity = GetActivityContext();
            activity?.SetTag(nameof(httpContext.Request.Method), httpContext.Request.Method);
            activity?.SetTag(nameof(httpContext.Request.Path), httpContext.Request.Method);
            activity?.SetTag(nameof(httpContext.Request.QueryString), httpContext.Request.QueryString);
            activity?.SetTag(nameof(httpContext.Request.Headers), FormatHeaders(httpContext.Request.Headers));
            activity?.SetTag(nameof(httpContext.Request.Host), httpContext.Request.Host);
            activity?.SetTag(nameof(httpContext.Request.Body), await ReadBodyFromRequest(httpContext.Request));
        }

        private static string GetTraceId()
        {
            return GetActivityContext()?.Id!;
        }

        private static Activity? GetActivityContext()
        {
            return Activity.Current?.Start();
        }


        private static string FormatHeaders(IHeaderDictionary headers)
        {
            return string.Join(", ",
                headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
        }

        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
            request.EnableBuffering();

            using StreamReader streamReader = new(request.Body, leaveOpen: true);
            string requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }
    }
}
