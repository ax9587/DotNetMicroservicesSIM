using DynamicExpresso;
using System;
using System.ComponentModel.DataAnnotations;
using static System.String;

namespace PricingSIMService.Model
{
    public class BasePremiumCalculationRule
    {
        public Guid Id { get; private set; }
        [Required]
        public string CoverCode { get; private set; }
        public string ApplyIfFormula { get; private set; }
        public string BasePriceFormula { get; private set; }

        public Tariff Tariff { get; set; }

        //{"CoverCode":"C1","ApplyIfFormula":null,"BasePriceFormula":"(NUM_OF_ADULTS) * (DESTINATION == \"EUR\" ? 26.00M : 34.00M)"}

        public BasePremiumCalculationRule()
        {
        }

        public BasePremiumCalculationRule(string coverCode, string applyIfFormula, string basePriceFormula)
        {
            Id = Guid.NewGuid();
            CoverCode = coverCode;
            ApplyIfFormula = applyIfFormula;
            BasePriceFormula = basePriceFormula;
        }

        public bool Applies(Cover cover, Calculation calculation)
        {
            if (cover.Code != CoverCode)
                return false;

            if (IsNullOrEmpty(ApplyIfFormula))
                return true;

            var (paramDefinitions,values) = calculation.ToCalculationParams();
            return (bool)new Interpreter()
                .Parse(ApplyIfFormula, paramDefinitions.ToArray())
                .Invoke(values.ToArray());
        }
        //{
        //    ProductCode = "TRI",
        //    PolicyFrom = DateTimeOffset.Now.AddDays(5),
        //    PolicyTo = DateTimeOffset.Now.AddDays(10),
        //    SelectedCovers = new List<string> { "C1", "C2", "C3" },
        //    Answers = new List<QuestionAnswer>
        //    {
        //        new NumericQuestionAnswer { QuestionCode = "NUM_OF_ADULTS", Answer = 1M},
        //        new NumericQuestionAnswer { QuestionCode = "NUM_OF_CHILDREN", Answer = 1M},
        //        new TextQuestionAnswer { QuestionCode = "DESTINATION", Answer = "EUR"}
        //    }
        //}

        public decimal CalculateBasePrice(Calculation calculation)
        {
            var (paramDefinitions, values) = calculation.ToCalculationParams();

            return (decimal)new Interpreter()
                .Parse(BasePriceFormula, paramDefinitions.ToArray())
                .Invoke(values.ToArray());
        }

    }
}
