using DynamicExpresso;
using static System.String;
using static System.Decimal;
using System;

namespace PricingSIMService.Model
{
    public abstract class DiscountMarkupRule
    {
        public Guid Id { get; set; }
        public string ApplyIfFormula { get; protected set; }
        public decimal ParamValue { get; protected set; }

        //public Guid TariffId { get; set; }
        public Tariff Tariff { get; set; }
        public bool Applies(Calculation calculation)
        {
            if (IsNullOrEmpty(ApplyIfFormula))
                return true;

            var (paramDefinitions,values) = calculation.ToCalculationParams();
            return (bool)new Interpreter()
                .Parse(ApplyIfFormula, paramDefinitions.ToArray())
                .Invoke(values.ToArray());
        }

        public abstract Calculation Apply(Calculation calculation);
    }
    public enum DiscountType
    {
        Percent
    }
    public class PercentMarkupRule : DiscountMarkupRule
    {

        public PercentMarkupRule()
        { }

        public PercentMarkupRule(string applyIfFormula, decimal paramValue)
        {
            Id = Guid.NewGuid();
            ApplyIfFormula = applyIfFormula;
            ParamValue = paramValue;
        }

        public override Calculation Apply(Calculation calculation)
        {
            foreach (var cover in calculation.Covers.Values)
            {
                var priceAfterMarkup = Round(cover.Price * ParamValue, 2);
                cover.SetPrice(priceAfterMarkup);
            }

            return calculation;
        }
    }
}
