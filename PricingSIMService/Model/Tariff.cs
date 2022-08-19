using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using PricingSIMService.Extensions;

namespace PricingSIMService.Model
{
    public class Tariff
    {
        public Guid Id { get; private set; }

        [Required]
        public string Code { get; private set; }
        [JsonProperty]
        public IList<BasePremiumCalculationRule> basePremiumRules;
        [JsonProperty]
        //public IList<PercentMarkupRule> PercentMarkupRules;
         public IList<DiscountMarkupRule> discountMarkupRules;

         [JsonIgnore]
         public BasePremiumCalculationRuleList BasePremiumRules => new BasePremiumCalculationRuleList(basePremiumRules);
         [JsonIgnore]
         public DiscountMarkupRuleList DiscountMarkupRules => new DiscountMarkupRuleList(discountMarkupRules);

        public Tariff(string code)
        {
            Id = Guid.NewGuid();
            Code = code;
            basePremiumRules = new List<BasePremiumCalculationRule>();
            //percentMarkupRules=new List<PercentMarkupRule>();
            discountMarkupRules = new List<DiscountMarkupRule>();
        }

        //public void AddBasePriceRule(string coverCode, string applyIfFormula, string basePriceFormula)
        //{
        //    basePremiumRules.Add(new BasePremiumCalculationRule(coverCode, applyIfFormula, basePriceFormula));
        //}

        //public decimal CalculateBasePriceFor(Cover cover, Calculation calculation)
        //{
        //    return BasePremiumRules
        //        .Where(r => r.Applies(cover, calculation))
        //        .Select(r => r.CalculateBasePrice(calculation))
        //        .FirstOrDefault();
        //}

        //public void AddPercentMarkup(string applyIfFormula, decimal markup)
        //{
        //    PercentMarkupRules.Add(new PercentMarkupRule(applyIfFormula, markup));
        //    //DiscountMarkupRules.Add(new PercentMarkupRule(applyIfFormula, markup));
        //}

        //public void ApplyDiscount(Calculation calculation)
        //{
        //    PercentMarkupRules
        //    //DiscountMarkupRules
        //        .Where(r => r.Applies(calculation))
        //        .ForEach(r => r.Apply(calculation));
        //}

        public Calculation CalculatePrice(Calculation calculation)
        {
            CalcBasePrices(calculation);
            ApplyDiscounts(calculation);
            UpdateTotals(calculation);
            return calculation;
        }

        

        private void CalcBasePrices(Calculation calculation)
        {
            foreach (var cover in calculation.Covers.Values)
            {
                cover.SetPrice(BasePremiumRules.CalculateBasePriceFor(cover,calculation));
            }
        }

        private void ApplyDiscounts(Calculation calculation)
        {
            DiscountMarkupRules.Apply(calculation);
        }

        private void UpdateTotals(Calculation calculation)
        {
            calculation.UpdateTotal();
        }
    }
}
