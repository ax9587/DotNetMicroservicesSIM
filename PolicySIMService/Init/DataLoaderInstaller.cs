using Microsoft.Extensions.DependencyInjection;


namespace PolicySIMService.Init
{
    public static class DataLoaderInstaller
    {
        public static IServiceCollection AddPolicyDemoInitializer(this IServiceCollection services)
        {
            services.AddScoped<DataLoader>();
            return services;
        }
    }
}
