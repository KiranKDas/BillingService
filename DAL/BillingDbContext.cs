using Microsoft.EntityFrameworkCore;
using BillingService.Models;

namespace BillingService.Data
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options) { }

        public DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>().HasKey(b => b.BillId);
        }
    }
}