using Microsoft.Extensions.DependencyInjection;
using PolicySIMService.RestClients;

namespace PolicyService.RestClients
{
    public static class RestClientsInstaller
    {
        public static IServiceCollection AddPricingRestClient(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IPricingClient), typeof(PricingClient));
            services.AddSingleton(typeof(IPricingService), typeof(PricingService));
            return services;
        }
    }
}
