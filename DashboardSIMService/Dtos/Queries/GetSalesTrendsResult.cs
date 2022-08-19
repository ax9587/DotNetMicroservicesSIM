using System.Collections.Generic;

namespace DashboardSIMService.Dtos.Queries
{
    public class GetSalesTrendsResult
    {
        public List<PeriodSaleDto> PeriodsSales { get; set; }
    }
}