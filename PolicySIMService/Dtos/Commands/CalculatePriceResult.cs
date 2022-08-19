using System.Collections.Generic;

namespace PolicySIMService.Dtos.Commands
{
    public class CalculatePriceResult
    {
        public decimal TotalPrice { get; set; }
        public Dictionary<string,decimal> CoverPrices { get; set; }

        public static CalculatePriceResult Empty() => new CalculatePriceResult { TotalPrice = 0M, CoverPrices = new Dictionary<string, decimal>() };
    }
}
