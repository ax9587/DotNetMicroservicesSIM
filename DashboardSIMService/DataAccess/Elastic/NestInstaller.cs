using System;
using DashboardSIMService.Domain;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace DashboardSIMService.DataAccess.Elastic
{
    public static class NestInstaller
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, string cnString)
        {
            services.AddSingleton(typeof(ElasticClient), svc => CreateElasticClient(cnString));
            services.AddScoped(typeof(IPolicyRepository), typeof(ElasticPolicyRepository));
            return services;
        }

        private static ElasticClient CreateElasticClient(string cnString)
        {
            var connectionSettings = new ConnectionSettings(new Uri(cnString))
                .EnableHttpCompression()
                .BasicAuthentication("elastic", "286Mh96KlSxxHJ2PZ8tr694z")
                .DefaultMappingFor<PolicyDocument>(m=>
                    m.IndexName("policy_lab_stats").IdProperty(d=>d.Number));
            return new ElasticClient(connectionSettings);
        }
    }
}
