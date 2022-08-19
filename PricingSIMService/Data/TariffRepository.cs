using Microsoft.EntityFrameworkCore;
using PricingSIMService.Model;
using ProductSIMService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingSIMService.Data
{
    public class TariffRepository : ITariffRepository
    {
        private readonly PricingContext _pricingDbContext;

        public TariffRepository(PricingContext pricingDbContext)
        {
            this._pricingDbContext = pricingDbContext ?? throw new ArgumentNullException(nameof(pricingDbContext));
        }

        public Task<Tariff> this[string code] => throw new NotImplementedException();

        public void Add(Tariff tariff)
        {
            _pricingDbContext.Tariffs.Add(tariff);
        }

        public async Task<bool> Exists(string code)
        {
            var tariff= await _pricingDbContext
                .Tariffs
                .FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());
            if (tariff != null)
            {
                return true;
            }

            return false;
        }

        public async Task<Tariff> WithCode(string code)
        {
            return await _pricingDbContext
                .Tariffs
                .Include(c => c.basePremiumRules)
                .Include(c => c.discountMarkupRules)
                .FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());
        }
    }
}
