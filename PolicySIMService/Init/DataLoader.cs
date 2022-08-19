using PolicySIMService.Data;
using System.Linq;

namespace PolicySIMService.Init
{
    public class DataLoader
    {

        private readonly PolicyContext dbContext;

        public DataLoader(PolicyContext context)
        {
            dbContext = context;
        }

        public void Seed()
        {
            dbContext.Database.EnsureCreated();
            if (dbContext.Offers.Any())
            {
                return;
            }

            dbContext.Offers.Add(DemoOfferFactory.DemoOffer());
            dbContext.Policys.Add(DemoPolicyFactory.DemoPolicy());
            

            dbContext.SaveChanges();
        }
    }
}
