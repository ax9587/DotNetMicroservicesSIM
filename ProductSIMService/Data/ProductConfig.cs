using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductSIMService.Model;

namespace ProductSIMService.Data
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            
            entity.ToTable("Product");
            entity.Property(q => q.Code).IsRequired();
            entity.Property(q => q.Name).IsRequired();            
        }
    }
}
