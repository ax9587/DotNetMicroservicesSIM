using System;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using PolicySearchSIMService.Model;
using PolicySearchSIMService.Data.ElasticSearch;
using PolicySearchSIMService.Dtos;
using PolicySearchSIMService.Dtos.Events;

namespace PolicySearchSIMService.Messaging.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PolicyPublished:
                    addPolicy(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch(eventType.Event)
            {
                case "POLICY_CREATED":
                    Console.WriteLine("--> Policy Published Event Detected");
                    return EventType.PolicyPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addPolicy(string policyPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPolicyRepository>();

                var policyDto = JsonSerializer.Deserialize<PolicyCreated>(policyPublishedMessage);

                try
                {
                   
                        repo.Add(new Policy
                        {
                            PolicyNumber = policyDto.PolicyNumber,
                            PolicyStartDate = policyDto.PolicyFrom,
                            PolicyEndDate = policyDto.PolicyTo,
                            ProductCode = policyDto.ProductCode,
                            PolicyHolder = $"{policyDto.PolicyHolder.FirstName} {policyDto.PolicyHolder.LastName}",
                            PremiumAmount = policyDto.TotalPremium,
                        });
                        Console.WriteLine("--> Policy added!");
                    
   

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Policy to Elastic {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PolicyPublished,
        PolicyTerminated,
        Undetermined
    }
}