using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PolicySIMService.Model;

namespace PolicySIMService.Data
{
    public class PolicyContext : DbContext
    {
        public PolicyContext (DbContextOptions<PolicyContext> options)
            : base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<ValidityPeriod> ValidityPeriods { get; set; }
        public DbSet<Policy> Policys { get; set; }
        public DbSet<PolicyCover> PolicyCovers { get; set; }
        public DbSet<PolicyHolder> PolicyHolders { get; set; }
        public DbSet<PolicyVersion> PolicyVersions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().ToTable("Address");

            modelBuilder.Entity<Offer>().ToTable("Offer");
            modelBuilder.Entity<Cover>().HasOne(c => c.Offer).WithMany(o => o.Covers);
           

            modelBuilder.Entity<Policy>().ToTable("Policy");
            modelBuilder.Entity<PolicyVersion>().HasOne(pv => pv.Policy).WithMany(p => p.versions);

        }
    }
}