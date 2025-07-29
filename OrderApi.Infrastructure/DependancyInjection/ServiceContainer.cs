using eCommerce.SharedLib.DependencyIncjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.DependecyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repository;


namespace OrderApi.Infrastructure.DependancyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices (this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:FileName"]!);
            services.AddScoped<IOrder, OrderRepository>();
            services.AddHttpContextAccessor();
            services.AddTransient<AuthorizedHandler>();
            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register middleware for global exception handling and other policies

            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
