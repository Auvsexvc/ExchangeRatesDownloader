using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DbDataWriter : IDbDataWriter
    {
        private readonly AppDbContext _appDbContext;

        public DbDataWriter(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task SaveToDbAsync(IEnumerable<ExchangeTable> downloadedTables)
        {
            foreach (var table in downloadedTables)
            {
                if (!_appDbContext.ExchangeTables.Any(t => t.No == table.No))
                {
                    await _appDbContext.AddAsync(table);

                    foreach (var rate in table.Rates)
                    {
                        await _appDbContext.AddAsync(rate);
                    }

                    await _appDbContext.SaveChangesAsync();
                }
            }
        }
    }
}