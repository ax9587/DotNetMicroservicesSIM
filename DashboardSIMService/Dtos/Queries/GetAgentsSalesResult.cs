using System.Collections.Generic;


namespace DashboardSIMService.Dtos.Queries
{
    public class GetAgentsSalesResult
    {
        public IDictionary<string, SalesDto> PerAgentTotal { get; set; }
    }
}