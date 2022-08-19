using System.Collections.Generic;
using System.Threading.Tasks;
using DashboardSIMService.Domain;
using DashboardSIMService.Dtos.Queries;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DashboardService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IPolicyRepository policyRepository;

        public DashboardController(IPolicyRepository policyRepository)
        {
            this.policyRepository = policyRepository;
        }
        //[DisableCors]
        //[EnableCors("CorsPolicy")]
        [HttpPost("agents-sales")]
        public async Task<GetAgentsSalesResult> AgentsSales([FromBody] GetAgentsSalesQuery query)
        {
            var queryResult = policyRepository.GetAgentSales
            (
                new AgentSalesQuery
                (
                    query.AgentLogin,
                    query.ProductCode,
                    query.SalesDateFrom,
                    query.SalesDateTo
                )
            );

            return await Task.FromResult(BuildResult(queryResult));
        }
        //[DisableCors]
        //[EnableCors("CorsPolicy")]
        [HttpPost("total-sales")]
        public async Task<GetTotalSalesResult> TotalSales([FromBody] GetTotalSalesQuery query)
        {
            var queryResult = policyRepository.GetTotalSales
            (
                new TotalSalesQuery
                (
                    query.ProductCode,
                    query.SalesDateFrom,
                    query.SalesDateTo
                )
            );

            return await Task.FromResult(BuildResult(queryResult));
        }
        //[DisableCors]
        //[EnableCors("CorsPolicy")]
        [HttpPost("sales-trends")]
        public async Task<GetSalesTrendsResult> SalesTrends([FromBody] GetSalesTrendsQuery query)
        {
            var queryResult = policyRepository.GetSalesTrend
            (
                new SalesTrendsQuery
                (
                    query.ProductCode,
                    query.SalesDateFrom,
                    query.SalesDateTo,
                    query.Unit.ToTimeAggregationUnit()
                )
            );

            return await Task.FromResult(BuildResult(queryResult));
        }

        private GetAgentsSalesResult BuildResult(AgentSalesQueryResult queryResult)
        {
            var result = new GetAgentsSalesResult
            {
                PerAgentTotal = new Dictionary<string, SalesDto>()
            };

            foreach (var agentResult in queryResult.PerAgentTotal)
            {
                result.PerAgentTotal[agentResult.Key] = new SalesDto
                {
                    PoliciesCount = agentResult.Value.PoliciesCount,
                    PremiumAmount = agentResult.Value.PremiumAmount
                };
            }

            return result;
        }
        private GetSalesTrendsResult BuildResult(SalesTrendsResult queryResult)
        {
            var result = new GetSalesTrendsResult
            {
                PeriodsSales = new List<PeriodSaleDto>()
            };

            foreach (var periodSale in queryResult.PeriodSales)
            {
                result.PeriodsSales.Add(new PeriodSaleDto
                {
                    PeriodDate = periodSale.PeriodDate,
                    Period = periodSale.Period,
                    Sales = new SalesDto(periodSale.Sales.PoliciesCount, periodSale.Sales.PremiumAmount)
                });
            }

            return result;

        }

        private GetTotalSalesResult BuildResult(TotalSalesQueryResult queryResult)
        {
            var result = new GetTotalSalesResult
            {
                Total = new SalesDto(queryResult.Total.PoliciesCount, queryResult.Total.PremiumAmount),
                PerProductTotal = new Dictionary<string, SalesDto>()
            };

            foreach (var productTotal in queryResult.PerProductTotal)
            {
                result.PerProductTotal[productTotal.Key] = new SalesDto
                (
                    productTotal.Value.PoliciesCount,
                    productTotal.Value.PremiumAmount
                );
            }

            return result;
        }
    }
}