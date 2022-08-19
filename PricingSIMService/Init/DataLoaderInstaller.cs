using Microsoft.Extensions.DependencyInjection;


namespace PricingSIMService.Init
{
    public static class DataLoaderInstaller
    {
        public static IServiceCollection AddPricingDemoInitializer(this IServiceCollection services)
        {
            services.AddScoped<DataLoader>();
            return services;
        }
    }
}
