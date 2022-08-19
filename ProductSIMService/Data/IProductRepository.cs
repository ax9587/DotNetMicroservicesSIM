using ProductSIMService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductSIMService.Data
{
    public interface IProductRepository
    {
        Task<Product> Add(Product product);

        Task<List<Product>> FindAllActive();

        Task<Product> FindOne(string productCode);
        
        Task<Product> FindById(Guid id);

        bool SaveChanges();
    }
}
