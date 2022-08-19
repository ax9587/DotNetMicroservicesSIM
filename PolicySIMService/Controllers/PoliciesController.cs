using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using PolicySIMService.Data;
using PolicySIMService.Dtos.Commands;
using PolicySIMService.Dtos.Commands.Events;
using PolicySIMService.Dtos.Queries;
using PolicySIMService.Messaging;
using PolicySIMService.Model;

namespace PolicySIMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyRepository _repository;
        private readonly IOfferRepository _orepository;
        private readonly IMessageBusClient _messageBus;
        public PoliciesController(IPolicyRepository repository, IOfferRepository orepository, IMessageBusClient messageBus)
        {
            _repository = repository;
            _orepository = orepository;
            _messageBus = messageBus;
        }
        /*
         {
  "offerNumber": "b366a976-95c7-4bb6-bc54-1cb3cdd6d07f",
  "policyHolder": {
    "firstName": "string",
    "lastName": "string",
    "taxId": "string"
  },
  "policyHolderAddress": {
    "country": "string",
    "zipCode": "string",
    "city": "string",
    "street": "string"
  }
}
         */
        // POST 
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePolicyCommand cmd)
        {

            var offer = await _orepository.WithNumber(cmd.OfferNumber);
            var customer = new PolicyHolder
            (
                cmd.PolicyHolder.FirstName,
                cmd.PolicyHolder.LastName,
                cmd.PolicyHolder.TaxId,
                Address.Of
                (
                    cmd.PolicyHolderAddress.Country,
                    cmd.PolicyHolderAddress.ZipCode,
                    cmd.PolicyHolderAddress.City,
                    cmd.PolicyHolderAddress.Street
                )
            );
            var policy = offer.Buy(customer);

            _repository.Add(policy);

            _repository.SaveChanges();

            _messageBus.PublishPolicyCreated(PolicyCreated(policy));

            //_repository.SaveChanges();

            return Ok(new CreatePolicyResult
            {
                PolicyNumber = policy.Number
            });
        }

        // GET 
        [HttpGet("{policyNumber}")]
        public async Task<ActionResult> Get(string policyNumber)
        {
           var result =await _repository.WithNumber(policyNumber);
            return Ok(ConstructResult(result));
        }

        // DELETE
        [HttpDelete("/terminate")]
        public async Task<ActionResult> Post([FromBody] TerminatePolicyCommand cmd)
        {
            var policy = await _repository.WithNumber(cmd.PolicyNumber);

            var terminationResult = policy.Terminate(cmd.TerminationDate);

            _messageBus.PublishPolicyTerminated(PolicyTerminated(terminationResult));

             _repository.SaveChanges();

            return Ok(new TerminatePolicyResult
            {
                PolicyNumber = policy.Number,
                MoneyToReturn = terminationResult.AmountToReturn
            });
        }

        private PolicyTerminated PolicyTerminated(PolicyTerminationResult terminationResult)
        {
            return new PolicyTerminated
            {
                Event= "POLICY_TERMINATED",
                PolicyNumber = terminationResult.TerminalVersion.Policy.Number,
                PolicyFrom = terminationResult.TerminalVersion.CoverPeriod.ValidFrom,
                PolicyTo = terminationResult.TerminalVersion.CoverPeriod.ValidTo,
                ProductCode = terminationResult.TerminalVersion.Policy.ProductCode,
                TotalPremium = terminationResult.TerminalVersion.TotalPremiumAmount,
                AmountToReturn = terminationResult.AmountToReturn,
                PolicyHolder = new PersonDto
                {
                    FirstName = terminationResult.TerminalVersion.PolicyHolder.FirstName,
                    LastName = terminationResult.TerminalVersion.PolicyHolder.LastName,
                    TaxId = terminationResult.TerminalVersion.PolicyHolder.Pesel
                }
            };
        }

        private static PolicyCreated PolicyCreated(Policy policy)
        {
            var version = policy.Versions.First(v => v.VersionNumber == 1);

            return new PolicyCreated
            {
                Event="POLICY_CREATED",
                PolicyNumber = policy.Number,
                PolicyFrom = version.CoverPeriod.ValidFrom,
                PolicyTo = version.CoverPeriod.ValidTo,
                ProductCode = policy.ProductCode,
                TotalPremium = version.TotalPremiumAmount,
                PolicyHolder = new PersonDto
                {
                    FirstName = version.PolicyHolder.FirstName,
                    LastName = version.PolicyHolder.LastName,
                    TaxId = version.PolicyHolder.Pesel
                },
                AgentLogin = policy.AgentLogin
            };
        }

        private GetPolicyDetailsQueryResult ConstructResult(Policy policy)
        {
            var effectiveVersion = policy.Versions.FirstVersion();

            return new GetPolicyDetailsQueryResult
            {
                Policy = new PolicyDetailsDto
                {
                    Number = policy.Number,
                    ProductCode = policy.ProductCode,
                    DateFrom = effectiveVersion.CoverPeriod.ValidFrom,
                    DateTo = effectiveVersion.CoverPeriod.ValidTo,
                    PolicyHolder = $"{effectiveVersion.PolicyHolder.FirstName} {effectiveVersion.PolicyHolder.LastName}",
                    TotalPremium = effectiveVersion.TotalPremiumAmount,

                    AccountNumber = null,
                    Covers = effectiveVersion.Covers.Select(c => c.Code).ToList()
                }
            };
        }
    }
}