using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySearchSIMService.Data.ElasticSearch
{
    public static class NestInstaller
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, string cnString)
        {
            services.AddSingleton(typeof(ElasticClient), svc => CreateElasticClient(cnString));
            services.AddScoped(typeof(IPolicyRepository), typeof(PolicyRepository));
            return services;
        }

        private static ElasticClient CreateElasticClient(string cnString)
        {
            var settings = new ConnectionSettings(new Uri(cnString))
                .EnableHttpCompression()
                .BasicAuthentication("elastic", "286Mh96KlSxxHJ2PZ8tr694z")
                .DefaultIndex("lab_policies");
            var client = new ElasticClient(settings);
            return client;
        }
    }
}
