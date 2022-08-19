using PolicySIMService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.Data
{
    public interface IOfferRepository
    {
        void Add(Offer offer);

        Task<Offer> WithNumber(string number);

        bool SaveChanges();
    }
}
