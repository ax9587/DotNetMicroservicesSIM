using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentSIMService.Model;

namespace PaymentSIMService.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext (DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public DbSet<PolicyAccount> PolicyAccounts { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<ExpectedPayment> ExpectedPayments { get; set; }

        public DbSet<InPayment> InPayments { get; set; }

        public DbSet<OutPayment> OutPayments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolicyAccount>().ToTable("PolicyAccount");
            modelBuilder.Entity<AccountingEntry>().ToTable("AccountingEntry")
                .HasDiscriminator<int>("EntryType")
                .HasValue<ExpectedPayment>(0)
                .HasValue<InPayment>(1)
                .HasValue<OutPayment>(2);
            modelBuilder.Entity<AccountingEntry>().HasOne(e => e.PolicyAccount).WithMany(p => p.Entries);
            modelBuilder.Entity<Owner>().ToTable("Owner");
        }
    }
}