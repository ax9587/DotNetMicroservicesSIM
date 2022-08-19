using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PolicySearchSIMService.Model;
using PolicySearchSIMService.Data.ElasticSearch;
using PolicySearchSIMService.Dtos.Queries;

namespace PolicySearchSIMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicySearchController : ControllerBase
    {
        private readonly IPolicyRepository _policis;

        public PolicySearchController(IPolicyRepository policis)
        {
            _policis=policis;
        }


        // GET api/values
        [HttpGet()]
        public async Task<ActionResult> SearchAsync([FromQuery] string q)
        {
            var searchResults = await _policis.Find(q);

            return Ok(FindPolicyResult(searchResults));
        }

        private FindPolicyResult FindPolicyResult(List<Policy> searchResults)
        {
            return new FindPolicyResult
            {
                Policies = searchResults.Select(p => new PolicyDto
                {
                    PolicyNumber = p.PolicyNumber,
                    PolicyStartDate = p.PolicyStartDate,
                    PolicyEndDate = p.PolicyEndDate,
                    ProductCode = p.ProductCode,
                    PolicyHolder = p.PolicyHolder,
                    PremiumAmount = p.PremiumAmount
                })
                .ToList()
            };
        }
    }
}
