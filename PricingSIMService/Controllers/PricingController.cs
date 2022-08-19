using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PricingService.Api.Commands;
using PricingSIMService.Data;
using PricingSIMService.Dtos.Commands;
using PricingSIMService.Model;

namespace PricingSIMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly ITariffRepository _repository;
        private readonly CalculatePriceCommandValidator commandValidator = new CalculatePriceCommandValidator();
        public PricingController(ITariffRepository repository)
        {
            _repository = repository;
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {

            Console.WriteLine("--> Getting Pricings....");

            var tariff = await _repository.WithCode("TRI");

            return Ok(tariff);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CalculatePriceCommand cmd)
        {
            //var result = await bus.Send(cmd);
            //return new JsonResult(result);
            commandValidator.ValidateAndThrow(cmd);

            var tariff = await _repository.WithCode(cmd.ProductCode);

            var calculation = tariff.CalculatePrice(ToCalculation(cmd));

            //{
            //    ProductCode = "TRI",
            //    PolicyFrom = DateTimeOffset.Now.AddDays(5),
            //    PolicyTo = DateTimeOffset.Now.AddDays(10),
            //    SelectedCovers = new List<string> { "C1", "C2", "C3" },
            //    Answers = new List<QuestionAnswer>
            //    {
            //        new NumericQuestionAnswer { QuestionCode = "NUM_OF_ADULTS", Answer = 1M},
            //        new NumericQuestionAnswer { QuestionCode = "NUM_OF_CHILDREN", Answer = 1M},
            //        new TextQuestionAnswer { QuestionCode = "DESTINATION", Answer = "EUR"}
            //    }
            //}

            return new JsonResult(ToResult(calculation));

            //Sample:
/*           
           {
                "productCode": "TRI",
  "policyFrom": "2021-10-21T20:07:20.937Z",
  "policyTo": "2021-10-21T20:07:20.937Z",
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
        }

        private static Calculation ToCalculation(CalculatePriceCommand cmd)
        {
            return new Calculation(
                cmd.ProductCode,
                cmd.PolicyFrom,
                cmd.PolicyTo,
                cmd.SelectedCovers,
                cmd.Answers.ToDictionary(a => a.QuestionCode, a => a.GetAnswer()));
        }

        private static CalculatePriceResult ToResult(Calculation calculation)
        {
            return new CalculatePriceResult
            {
                TotalPrice = calculation.TotalPremium,
                CoverPrices = calculation.Covers.ToDictionary(c => c.Key, c => c.Value.Price)
            };
        }

    }    
}
