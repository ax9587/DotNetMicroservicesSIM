using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.Model
{
    public class ValidityPeriod : ICloneable
    {
        public virtual Guid Id { get; protected set; }
        public virtual DateTime ValidFrom { get; protected set; }
        public virtual DateTime ValidTo { get; protected set; }


        public ValidityPeriod(DateTime validFrom, DateTime validTo)
        {
            Id = Guid.NewGuid();
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        protected ValidityPeriod() { } //NH required

        public static ValidityPeriod Between(DateTime validFrom, DateTime validTo)
            => new ValidityPeriod(validFrom, validTo);

        public ValidityPeriod Clone()
        {
            return new ValidityPeriod(ValidFrom, ValidTo);
        }

        public bool Contains(DateTime theDate)
        {
            if (theDate > ValidTo)
                return false;

            if (theDate < ValidFrom)
                return false;
            
            return true;
        }

        public ValidityPeriod EndOn(DateTime endDate)
        {
            return new ValidityPeriod(ValidFrom, endDate);
        }

        public int Days => ValidTo.Subtract(ValidFrom).Days;
        
        object ICloneable.Clone()
        {
            return Clone();
        }

    }
}
