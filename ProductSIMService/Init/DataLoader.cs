using ProductSIMService.Data;
using System.Linq;

namespace ProductSIMService.Init
{
    public class DataLoader
    {

        private readonly ProductContext dbContext;

        public DataLoader(ProductContext context)
        {
            dbContext = context;
        }

        public void Seed()
        {
            dbContext.Database.EnsureCreated();
            if (dbContext.Products.Any())
            {
                return;
            }

            dbContext.Products.Add(DemoProductFactory.Travel());
            dbContext.Products.Add(DemoProductFactory.House());
            dbContext.Products.Add(DemoProductFactory.Farm());
            dbContext.Products.Add(DemoProductFactory.Car());

            dbContext.SaveChanges();
        }
    }
}
