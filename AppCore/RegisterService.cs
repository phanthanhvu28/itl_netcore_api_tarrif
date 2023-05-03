using AppCore.PipelineBehaviors;
using AppCore.RestClients;
using AppCore.Settings;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Libraries.RestClientBase;

namespace AppCore
{
    public static class RegisterService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
        {

            // TODO: test baseUri
            services.AddRestEase(typeof(IAdminSettingApi), configuration[ApiServices.AdminService.Url]!);
            services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);

            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));//Instance init created each time service


            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

           

            return services;
        }
    }
}