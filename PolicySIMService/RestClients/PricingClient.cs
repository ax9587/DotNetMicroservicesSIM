using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PolicySIMService.Dtos.Commands;
using Polly;
using Polly.Retry;
using RestEase;

namespace PolicySIMService.RestClients
{

  
        public interface IPricingClient
        {
            [Post]
            Task<CalculatePriceResult> CalculatePrice([Body] CalculatePriceCommand cmd);
        }


    public class PricingClient : IPricingClient
        {
            private readonly IPricingClient client;

            private static AsyncRetryPolicy retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));

            public PricingClient(IConfiguration configuration)
            {
            
                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(configuration.GetValue<string>("PricingService"))
                };
                client = RestClient.For<IPricingClient>(httpClient);
            }

            public Task<CalculatePriceResult> CalculatePrice([Body] CalculatePriceCommand cmd)
            {
                return retryPolicy.ExecuteAsync(async () => await client.CalculatePrice(cmd));
            }
        }
    }

