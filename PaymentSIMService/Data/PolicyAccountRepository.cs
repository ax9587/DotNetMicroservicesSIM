using Microsoft.EntityFrameworkCore;
using PaymentSIMService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSIMService.Data
{
    public class PolicyAccountRepository : IPolicyAccountRepository
    {
        private readonly PaymentContext _paymentDbContext;

        public PolicyAccountRepository(PaymentContext paymentDbContext)
        {
            this._paymentDbContext = paymentDbContext ?? throw new ArgumentNullException(nameof(paymentDbContext));
        }
        public async void Add(PolicyAccount policyAccount)
        {
            await _paymentDbContext.PolicyAccounts.AddAsync(policyAccount);
        }

        public Task<bool> ExistsWithPolicyNumber(string policyNumber)
        {
            return  _paymentDbContext.PolicyAccounts.AnyAsync(p => p.PolicyNumber == policyNumber);
        }

        public async Task<PolicyAccount> FindByNumber(string policyNumber)
        {
            return _paymentDbContext.PolicyAccounts.FirstOrDefault(p => p.PolicyNumber == policyNumber);
        }

        public void Update(PolicyAccount policyAccount)
        {
            var entity = _paymentDbContext.PolicyAccounts.FirstOrDefault(p => p.PolicyNumber == policyAccount.PolicyNumber);

            if (entity != null)
            {
                entity = policyAccount;
            }
        }
        public bool SaveChanges()
        {
            return (_paymentDbContext.SaveChanges() >= 0);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //session.Dispose();
            }

        }
    }
}
