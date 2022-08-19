using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardSIMService.Dtos.Events
{
    public class PolicyCreated
    {
        public string Event { get; set; }
        public string PolicyNumber { get; set; }
        public string ProductCode { get; set; }
        public DateTime PolicyFrom { get; set; }
        public DateTime PolicyTo { get; set; }
        public PersonDto PolicyHolder { get; set; }
        public decimal TotalPremium { get; set; }
        public string AgentLogin { get; set; }
    }
}
