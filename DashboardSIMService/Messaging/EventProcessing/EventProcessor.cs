using System;
using System.Text.Json;
using System.Threading.Tasks;
using DashboardSIMService.Domain;
using DashboardSIMService.Dtos;
using DashboardSIMService.Dtos.Events;
using Microsoft.Extensions.DependencyInjection;


namespace DashboardSIMService.Messaging.EventProcessing
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
                    addPolicyAsync(message);
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
                //case "POLICY_TERMINATED":
                    //Console.WriteLine("--> Policy Terminated Event Detected");
                    //return EventType.PolicyTerminated;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task addPolicyAsync(string policyPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPolicyRepository>();


                var policyDto = JsonSerializer.Deserialize<PolicyCreated>(policyPublishedMessage);

                try
                {
                    var policy = new PolicyDocument
                        (
                         policyDto.PolicyNumber,
                         policyDto.PolicyFrom,
                         policyDto.PolicyTo,
                         $"{policyDto.PolicyHolder.FirstName} {policyDto.PolicyHolder.LastName}",
                         policyDto.ProductCode,
                         policyDto.TotalPremium,
                         policyDto.AgentLogin
                       );
                        repo.Save(policy);
                        Console.WriteLine("--> Policy added!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Policy to Payment {ex.Message}");
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