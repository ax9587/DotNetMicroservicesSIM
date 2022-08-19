
using PaymentSIMService.Data;
using System.Linq;

namespace PaymentSIMService.Init
{
    public class DataLoader
    {

        private readonly PaymentContext dbContext;

        public DataLoader(PaymentContext context)
        {
            dbContext = context;
        }

        public void Seed()
        {
            dbContext.Database.EnsureCreated();
            if (dbContext.PolicyAccounts.Any())
            {
                return;
            }

            dbContext.PolicyAccounts.AddRange(DemoAccountsFactory.DemoAccounts());

            dbContext.SaveChanges();
        }
    }
}
