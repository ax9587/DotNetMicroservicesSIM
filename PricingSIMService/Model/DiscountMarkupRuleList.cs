using System.Collections.Generic;
using System.Linq;
using PricingSIMService.Extensions;

namespace PricingSIMService.Model
{
    public class DiscountMarkupRuleList
    {
        private readonly IList<DiscountMarkupRule> rules;

        public DiscountMarkupRuleList(IList<DiscountMarkupRule> rules)
        {
            this.rules = rules;
        }

        public void AddPercentMarkup(string applyIfFormula, decimal markup)
        {
            rules.Add(new PercentMarkupRule(applyIfFormula, markup));
        }

        public void Apply(Calculation calculation)
        {
            rules
                .Where(r => r.Applies(calculation))
                .ForEach(r => r.Apply(calculation));
        }
    }
}
