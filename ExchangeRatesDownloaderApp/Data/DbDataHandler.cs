using ExchangeRatesDownloaderApp.Entities;
using ExchangeRatesDownloaderApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DbDataHandler : IDbDataHandler
    {
        private readonly AppDbContext _appDbContext;

        public DbDataHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<ExchangeTable>> GetRecentAsync()
        {
            return await _appDbContext
                .ExchangeTables
                .Include(x => x.Rates)
                .GroupBy(x => x.Type)
                .Select(x => x.OrderByDescending(x => x.EffectiveDate).First())
                .ToListAsync();
        }

        public async Task SaveToDbAsync(ExchangeTable exchangeTable)
        {
            await _appDbContext.AddAsync(exchangeTable);

            await _appDbContext.AddRangeAsync(exchangeTable.Rates);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> CanConnectToDbAsync()
        {
            return await _appDbContext.Database.CanConnectAsync();
        }
    }
}