using Microsoft.EntityFrameworkCore;
using PolicySIMService.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.Data
{
    public class OfferRepository : IOfferRepository
    {
        private readonly PolicyContext _policyDbContext;

        public OfferRepository(PolicyContext policyDbContext)
        {
            this._policyDbContext = policyDbContext ?? throw new ArgumentNullException(nameof(policyDbContext));
        }
        public void Add(Offer offer)
        {
            _policyDbContext.Offers.Add(offer);
        }

        public async Task<Offer> WithNumber(string number)
        {
            return await _policyDbContext
             .Offers
             .Include(o => o.Covers)
             .Include(o=>o.PolicyValidityPeriod)
             .Include(o=>o.PolicyHolder)
             .Where(o => o.Number  == number)
             .SingleOrDefaultAsync();
        }

        public bool SaveChanges()
        {
            return (_policyDbContext.SaveChanges() >= 0);
        }
    }
}
