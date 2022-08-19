using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PaymenthSIMService.Dtos;
using PaymentSIMService.Data;
using PaymentSIMService.Dtos.Events;
using PaymentSIMService.Model;

namespace PaymentSIMService.Messaging.EventProcessing
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
                case EventType.PolicyTerminated:
                    terminatePolicyAsync(message);
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
                case "POLICY_TERMINATED":
                    Console.WriteLine("--> Policy Terminated Event Detected");
                    return EventType.PolicyTerminated;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task addPolicyAsync(string policyPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPolicyAccountRepository>();

                var accGen = scope.ServiceProvider.GetRequiredService<PolicyAccountNumberGenerator>();

                var policyDto = JsonSerializer.Deserialize<PolicyCreated>(policyPublishedMessage);

                try
                {
                    var policy = new PolicyAccount
                        (
                         policyDto.PolicyNumber,
                         accGen.Generate(),
                         policyDto.PolicyHolder.FirstName,
                         policyDto.PolicyHolder.LastName
                       );
                    var isExist = await repo.ExistsWithPolicyNumber(policyDto.PolicyNumber);
                    if (isExist)
                    {
                        Console.WriteLine("--> Policy already exist!");
                    }
                    else
                    {
                        repo.Add(policy);
                        repo.SaveChanges();
                        Console.WriteLine("--> Policy added!");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Policy to Payment {ex.Message}");
                }
            }
        }

        private async Task terminatePolicyAsync(string policyPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPolicyAccountRepository>();

                var policyDto = JsonSerializer.Deserialize<PolicyTerminated>(policyPublishedMessage);

                try
                {
                    var policyAccount = await repo.FindByNumber(policyDto.PolicyNumber);

                    policyAccount.Close(policyDto.PolicyTo, policyDto.TotalPremium);//TODO Check

                    repo.Update(policyAccount);
                    repo.SaveChanges();

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