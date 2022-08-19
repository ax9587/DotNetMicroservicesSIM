using Microsoft.Extensions.DependencyInjection;


namespace ProductSIMService.Init
{
    public static class DataLoaderInstaller
    {
        public static IServiceCollection AddProductDemoInitializer(this IServiceCollection services)
        {
            services.AddScoped<DataLoader>();
            return services;
        }
    }
}
