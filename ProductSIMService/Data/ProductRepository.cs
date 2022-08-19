using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductSIMService.Model;

namespace ProductSIMService.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _productDbContext;

        public ProductRepository(ProductContext productDbContext)
        {
            this._productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
        }

        public async Task<Product> Add(Product product)
        {
            await _productDbContext.Products.AddAsync(product);
            return product;
        }

        public async Task<List<Product>> FindAllActive()
        {
           return await _productDbContext
               .Products
               .Include(c => c.Covers)
               .Include("Questions.Choices")
               .Where(p => p.Status == ProductStatus.Active)
               .ToListAsync();
        }

        public async Task<Product> FindOne(string productCode)
        {
            return await _productDbContext
                .Products
                .Include(c => c.Covers)
                .Include("Questions.Choices")
                //.FirstOrDefaultAsync(p => p.Code.Equals(productCode, StringComparison.InvariantCultureIgnoreCase));
                .FirstOrDefaultAsync(p => p.Code.ToUpper()== productCode.ToUpper()) ;
        }

        public async Task<Product> FindById(Guid id)
        {
            return await _productDbContext.Products.Include(c => c.Covers).Include("Questions.Choices")
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_productDbContext.SaveChanges() >= 0);
        }
    }
}
