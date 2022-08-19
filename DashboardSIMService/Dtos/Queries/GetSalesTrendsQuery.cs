using System;


namespace DashboardSIMService.Dtos.Queries
{
    public class GetSalesTrendsQuery 
    {
        public string ProductCode { get; set; }
        public DateTime SalesDateFrom { get; set; }
        public DateTime SalesDateTo { get; set; }
        public TimeUnit Unit { get; set; }
    }
}