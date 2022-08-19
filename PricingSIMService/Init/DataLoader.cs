using ProductSIMService.Data;
using System.Linq;

namespace PricingSIMService.Init
{
    public class DataLoader
    {

        private readonly PricingContext dbContext;

        public DataLoader(PricingContext context)
        {
            dbContext = context;
        }

        public void Seed()
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.Tariffs.Any())
            {
                return;
            }

            dbContext.Tariffs.Add(DemoTariffFactory.Travel());
            dbContext.Tariffs.Add(DemoTariffFactory.House());
            dbContext.Tariffs.Add(DemoTariffFactory.Farm());
            dbContext.Tariffs.Add(DemoTariffFactory.Car());

            dbContext.SaveChanges();
        }
    }
}
