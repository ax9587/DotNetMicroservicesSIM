using PolicySIMService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.RestClients
{
    public interface IPricingService
    {
        Task<Price> CalculatePrice(PricingParams pricingParams);
    }

    public class PricingParams
    {
        public string ProductCode { get; set; }
        public DateTime PolicyFrom { get; set; }
        public DateTime PolicyTo { get; set; }
        public List<string> SelectedCovers { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
