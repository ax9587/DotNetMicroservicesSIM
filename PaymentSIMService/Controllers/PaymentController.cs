using Microsoft.AspNetCore.Mvc;
using PaymentSIMService.Data;
using PaymentSIMService.Dtos;
using PaymentSIMService.Exceptions;
using System;
using System.Threading.Tasks;


namespace PaymentSIMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPolicyAccountRepository _repo;

        public PaymentController(IPolicyAccountRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet("accounts/{policyNumber}")]
        public async Task<ActionResult> AccountBalance(string policyNumber)
        {
            var policyAccount = await _repo.FindByNumber(policyNumber);

            if (policyAccount == null)
            {
                throw new PolicyAccountNotFound(policyNumber);
            }

            return Ok(new GetAccountBalanceQueryResult
            {
                Balance = new PolicyAccountBalanceDto
                {
                    PolicyNumber = policyAccount.PolicyNumber,
                    PolicyAccountNumber = policyAccount.PolicyAccountNumber,
                    Balance = policyAccount.BalanceAt(DateTimeOffset.Now)
                }
            });
        }
    }
}