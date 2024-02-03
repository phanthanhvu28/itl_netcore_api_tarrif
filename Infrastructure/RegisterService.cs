using AppCore.Contracts.Common;
using AppCore.Contracts.RepositoryBase;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infrastructure
{
    public static class RegisterService
    {

        private const int DefaultMaxRetry = 1;
        private static readonly TimeSpan DefaultMaxTimeRetry = TimeSpan.FromSeconds(3);

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
       IConfiguration configuration)
        {
            services.AddEfContextPool(configuration);

            // services.Scan(scan =>
            // {
            //     scan.FromAssembliesOf(typeof(IGenericRepository<>))
            //         .AddClasses(classes => classes.AssignableTo(typeof(IGenericRepository<>)))
            //         .AsImplementedInterfaces()
            //         .WithScopedLifetime();
            // });

            services.AddScoped<IDTCostingMainRepository, DTCostingMainRepository>();
            services.AddScoped<IDTCostingCapacityRepository, DTCostingCapacityRepository>();

            return services;
        }

        private static void AddEfContextPool(this IServiceCollection services,
        IConfiguration configuration)
        {
            //services.AddDbContextPool<MainDbContext>(options =>
            //{
            //    string? connectionString = configuration.GetConnectionString(Database.Name);
            //    MySqlServerVersion version = new(Database.Version);
            //    options
            //        .UseLazyLoadingProxies()
            //        .UseMySql(connectionString, version,
            //            options =>
            //                options.UseNewtonsoftJson()
            //                    .EnableRetryOnFailure(
            //                        DefaultMaxRetry,
            //                        DefaultMaxTimeRetry,
            //                        null)
            //        );
            //});
            string? connectionString = configuration.GetConnectionString(DatabaseSQLSVR.Name);

            services.AddDbContextPool<MainDbContext>(options =>

                options.UseSqlServer(configuration.GetConnectionString(connectionString ?? ""))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );

            services.AddScoped<IDbFacade>(p => p.GetRequiredService<MainDbContext>());
        }

        public static WebApplication MigrationDatabase(this WebApplication builder)
        {
            try
            {
                using IServiceScope scope = builder.Services.CreateScope();
                IDbFacade provider = scope.ServiceProvider.GetRequiredService<IDbFacade>();
                if (builder.Environment.IsDevelopment() &&
                    Convert.ToBoolean(GlobalConfiguration.Configuration["Database:AllowClear"]))
                {
                    provider.Database.EnsureDeleted();
                    provider.Database.EnsureCreated();
                }

                provider.Database.Migrate();
            }
            catch (Exception e)
            {
                Log.Error("Start WebApp migration error {@Error} {@Stack}", e.Message, e.StackTrace);
            }

            return builder;
        }
    }
}