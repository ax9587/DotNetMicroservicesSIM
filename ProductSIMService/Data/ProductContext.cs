using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductSIMService.Model;

namespace ProductSIMService.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext (DbContextOptions<ProductContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<Choice> Choices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());

            modelBuilder.Entity<Cover>().HasOne(p => p.Product).WithMany(c => c.Covers);
            modelBuilder.Entity<Cover>().Property(c => c.Code).IsRequired();
            modelBuilder.Entity<Cover>().Property(c => c.Name).IsRequired();

            modelBuilder.Entity<Question>().HasOne(p => p.Product).WithMany(p => p.Questions);
            modelBuilder.Entity<Question>().Property(q => q.Code).IsRequired();
            modelBuilder.Entity<Question>().Property(q => q.Index).IsRequired();

            modelBuilder.Entity<Question>()
            .HasDiscriminator<int>("QuestionType")
            .HasValue<Question>(0)
            .HasValue<NumericQuestion>(1)
            .HasValue<DateQuestion>(2)
            .HasValue<ChoiceQuestion>(3);

            modelBuilder.Entity<Choice>().HasKey("Code");
            modelBuilder.Entity<Choice>().HasOne(q => q.Question).WithMany(c => c.Choices);
        }
    }
}