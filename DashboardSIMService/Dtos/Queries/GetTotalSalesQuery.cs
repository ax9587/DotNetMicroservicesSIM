using System;


namespace DashboardSIMService.Dtos.Queries
{
    public class GetTotalSalesQuery 
    {
        public string ProductCode { get; set; }
        public DateTime SalesDateFrom { get; set; }
        public DateTime SalesDateTo { get; set; }
    }
}