using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PricingSIMService.Model;

namespace ProductSIMService.Data
{
    public class PricingContext : DbContext
    {
        public PricingContext (DbContextOptions<PricingContext> options)
            : base(options)
        {
        }
        public DbSet<Tariff> Tariffs { get; set; }

        public DbSet<BasePremiumCalculationRule> BasePremiumCalculationRules { get; set; }

        public DbSet<PercentMarkupRule> PercentMarkupRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tariff>().ToTable("Tariff");
            modelBuilder.Entity<BasePremiumCalculationRule>().ToTable("BasePremiumRule");
            modelBuilder.Entity<BasePremiumCalculationRule>().HasOne(t => t.Tariff).WithMany(p => p.basePremiumRules);
            modelBuilder.Entity<DiscountMarkupRule>().ToTable("DiscountMarkupRule")
                .HasDiscriminator<int>("DiscountType")
                .HasValue<PercentMarkupRule>(1);
            //modelBuilder.Entity<PercentMarkupRule>().HasOne(t => t.Tariff).WithMany(p => p.percentMarkupRules);
            modelBuilder.Entity<DiscountMarkupRule>().HasOne(t => t.Tariff).WithMany(p => p.discountMarkupRules);
        }
    }
}