using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RestEase;
using RestEase.HttpClientFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Libraries.RestClientBase
{
    public static class UseRestEase
    {
        public static IServiceCollection AddRestEase(this IServiceCollection services,
            Type httpClientApi,
            string url = "http://localhost")
        {
            services.TryAddScoped<InvocationHandler>();
            services.AddRestEaseClient(httpClientApi, url,
                    client => client.RequestPathParamSerializer = new StringEnumRequestPathParamSerializer())
                .AddHttpMessageHandler<InvocationHandler>();

            return services;
        }
    }
}
