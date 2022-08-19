using PaymentSIMService.Model;
using System;
using System.Threading.Tasks;

namespace PaymentSIMService.Data
{
    public interface IPolicyAccountRepository:IDisposable
    {
        void Add(PolicyAccount policyAccount);

        void Update(PolicyAccount policyAccount);

        Task<PolicyAccount> FindByNumber(string accountNumber);
        Task<bool> ExistsWithPolicyNumber(string policyNumber);

        bool SaveChanges();
    }
}