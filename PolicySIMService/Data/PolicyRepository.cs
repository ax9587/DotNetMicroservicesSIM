using Microsoft.EntityFrameworkCore;
using PolicySIMService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.Data
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly PolicyContext _policyDbContext;
        public PolicyRepository(PolicyContext policyDbContext)
        {
            this._policyDbContext = policyDbContext ?? throw new ArgumentNullException(nameof(policyDbContext));
        }
        public void Add(Policy policy)
        {
            _policyDbContext.Policys.Add(policy);
        }

        public async Task<Policy> WithNumber(string number)
        {
            return await _policyDbContext
             .Policys
             .Include(p => p.Versions).ThenInclude(v => v.PolicyHolder)
             .Include(p => p.Versions).ThenInclude(v => v.CoverPeriod)
             .Where(o => o.Number == number)
             .SingleOrDefaultAsync();
        }

        public bool SaveChanges()
        {
            return _policyDbContext.SaveChanges() >= 0;
        }
    }
}
