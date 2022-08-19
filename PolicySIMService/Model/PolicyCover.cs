using System;

namespace PolicySIMService.Model
{
    public class PolicyCover
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual decimal Premium { get; protected set; }
        public virtual ValidityPeriod CoverPeriod { get; protected set; }

        public PolicyCover(Cover cover, ValidityPeriod coverPeriod)
        {
            Id = Guid.NewGuid();
            Code = cover.Code;
            Premium = cover.Price;
            CoverPeriod = coverPeriod;
        }
        
        protected PolicyCover() { } //NH required
        
        public PolicyCover EndOn(DateTime endDate)
        {
            var originalDaysCovered = CoverPeriod.Days;
            var daysNotUsed = originalDaysCovered - CoverPeriod.EndOn(endDate).Days;
            var premium = decimal.Round
            (
                this.Premium - (this.Premium * decimal.Divide(daysNotUsed,originalDaysCovered))
                , 2
            );
            
            return new PolicyCover
            {
                Code = this.Code,
                Premium = premium,
                CoverPeriod = this.CoverPeriod.EndOn(endDate)
            };
        }


    }
}