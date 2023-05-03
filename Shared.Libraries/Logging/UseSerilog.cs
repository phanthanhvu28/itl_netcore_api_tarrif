using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.Libraries.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Sinks.Grafana.Loki;

namespace Shared.Libraries.Logging
{
    public static class UseSerilog
    {
        private const string OutputTemplate =
       "[{Timestamp:dd-MM-yyyy HH:mm:ss}] [{Level:u3}] [{Environment}] [{ServiceName}] [{TraceId}] {Message}{NewLine}{Exception}";

        public static readonly string DefaultTraceId = ActivityTraceId.CreateRandom().ToString();

        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder,
        IConfiguration configuration)
        {
            try
            {
                string serviceName = builder.GetServiceName();
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty(Constants.Serilog.Environment, builder.Environment.EnvironmentName)
                    .Enrich.WithProperty(Constants.Serilog.TraceId, DefaultTraceId)
                    .Enrich.WithProperty(Constants.Serilog.Service, serviceName)
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentName()
                    .WriteTo.GrafanaLoki(
                        configuration[Grafana.LokiUrl]!,
                        GetLogLabel(builder.Environment.EnvironmentName, serviceName),
                        credentials: null)
                    .WriteTo.Console(outputTemplate: OutputTemplate)
                    .CreateLogger();

                builder.Host.UseSerilog();
                return builder;
            }
            catch(Exception ex)
            {
                return builder;
            }
        }

        private static string GetServiceName(this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            return configuration[Service.Name] ?? builder.Environment.ApplicationName;
        }

        private static List<LokiLabel> GetLogLabel(string environmentName, string serviceName)
        {
            List<LokiLabel> list = new()
        {
            new()
            {
                Key = environmentName,
                Value = serviceName
            }
        };
            return list;
        }

    }
}
