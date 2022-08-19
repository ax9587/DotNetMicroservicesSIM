using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PolicySIMService.Data;
using PolicySIMService.Dtos.Commands;
using PolicySIMService.Model;
using PolicySIMService.RestClients;
using static System.String;

namespace PolicySIMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOfferRepository _repository;
        private readonly IPricingService _pricingService;

        public OffersController(IOfferRepository repository,IPricingService pricingService)
        {
            _repository = repository;
            _pricingService = pricingService;
        }
       
        /*
         Sample
        {
  "productCode": "TRI",
  "policyFrom": "2021-10-25T22:15:02.569Z",
  "policyTo": "2021-10-25T22:15:02.569Z",
  "selectedCovers": [
    "C1"
  ],
  "answers": [
    {
      "questionCode": "DESTINATION",
      "questionType": 0,
      "answer":"EUR"
    },
     {
      "questionCode": "NUM_OF_ADULTS",
      "questionType": 1,
      "answer":1
    }
  ]
}
        */
        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateOfferCommand cmd, [FromHeader] string AgentLogin)
        {
            //calculate price
            var priceParams = ConstructPriceParams(cmd);
            var price = await _pricingService.CalculatePrice(priceParams);
            if (IsNullOrWhiteSpace(AgentLogin))
            {
                var o = Offer.ForPrice(
               priceParams.ProductCode,
               priceParams.PolicyFrom,
               priceParams.PolicyTo,
               null,
               price
               );
                _repository.Add(o);
               _repository.SaveChanges();

                return Ok(ConstructResult(o));
            }
            else
            {

                var o = Offer.ForPriceAndAgent(
                    priceParams.ProductCode,
                    priceParams.PolicyFrom,
                    priceParams.PolicyTo,
                    null,
                    price,
                    AgentLogin
                );
                _repository.Add(o);
                _repository.SaveChanges();

                return Ok(ConstructResult(o));
            }

            //var result = IsNullOrWhiteSpace(AgentLogin) ? await bus.Send(cmd) : await bus.Send(new CreateOfferByAgentCommand(AgentLogin, cmd));
            //return new JsonResult(result);
            return new JsonResult(null);
        }

        private CreateOfferResult ConstructResult(Offer o)
        {
            return new CreateOfferResult
            {
                OfferNumber = o.Number,
                TotalPrice = o.TotalPrice,
                CoversPrices = o.Covers.ToDictionary(c => c.Code, c => c.Price)
            };
        }

        private PricingParams ConstructPriceParams(CreateOfferCommand request)
        {
            return new PricingParams
            {
                ProductCode = request.ProductCode,
                PolicyFrom = request.PolicyFrom,
                PolicyTo = request.PolicyTo,
                SelectedCovers = request.SelectedCovers,
                Answers = request.Answers.Select(a => Answer.Create(a.QuestionType, a.QuestionCode, a.GetAnswer())).ToList()
            };
        }
    }
}
