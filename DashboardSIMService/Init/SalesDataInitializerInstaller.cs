using Microsoft.Extensions.DependencyInjection;

namespace DashboardSIMService.Init
{
    public static class SalesDataInitializerInstaller
    {
        public static IServiceCollection AddInitialSalesData(this IServiceCollection services)
        {
            services.AddScoped<SalesData>();
            services.AddHostedService<SalesDataInitializer>();
            return services;
        }
    }
}