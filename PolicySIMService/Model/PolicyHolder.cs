using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.Model
{
    public class PolicyHolder
    {
        public virtual Guid Id { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string Pesel { get; protected set; }
        public virtual Address Address { get; protected set; }

        public PolicyHolder(string firstName, string lastName, string pesel, Address address)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Pesel = pesel;
            Address = address;
        }

        protected PolicyHolder() { } //NH required
    }
}
