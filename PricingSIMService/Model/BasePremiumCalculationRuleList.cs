using System.Collections.Generic;
using System.Linq;

namespace PricingSIMService.Model
{
    public class BasePremiumCalculationRuleList
    {
        private readonly IList<BasePremiumCalculationRule> rules;

        public BasePremiumCalculationRuleList(IList<BasePremiumCalculationRule> rules)
        {
            this.rules = rules;
        }

        public void AddBasePriceRule(string coverCode, string applyIfFormula, string basePriceFormula)
        {
            rules.Add(new BasePremiumCalculationRule(coverCode, applyIfFormula, basePriceFormula));
        }

        public decimal CalculateBasePriceFor(Cover cover, Calculation calculation)
        {
            return rules
                .Where(r => r.Applies(cover,calculation))
                .Select(r => r.CalculateBasePrice(calculation))
                .FirstOrDefault();
        }
    }
}