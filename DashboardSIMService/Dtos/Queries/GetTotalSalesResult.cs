using System.Collections.Generic;


namespace DashboardSIMService.Dtos.Queries
{
    public class GetTotalSalesResult
    {
        public SalesDto Total { get; set; }
        
        public Dictionary<string,SalesDto> PerProductTotal { get; set; }
    }
}