using ExchangeRatesDownloaderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesDownloaderApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ExchangeTable> ExchangeTables => Set<ExchangeTable>();
        public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRate>().Property(e => e.Ask).HasPrecision(18, 9);
            modelBuilder.Entity<ExchangeRate>().Property(e => e.Bid).HasPrecision(18, 9);
            modelBuilder.Entity<ExchangeRate>().Property(e => e.Mid).HasPrecision(18, 9);

            base.OnModelCreating(modelBuilder);
        }
    }
}