using ExchangeRatesDownloaderApp.Extensions;
using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
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

        public async Task<IEnumerable<ExchangeTableDto>> GetTablesAsync()
        {
            var data = await _appDbContext
                .ExchangeTables
                .Include(x => x.Rates)
                .GroupBy(x => x.Type)
                .Select(x => x.OrderByDescending(x => x.EffectiveDate).First().ToDto())
                .ToListAsync();

            return data;
        }

        public async Task SaveTableWithRatesToDbAsync(ExchangeTableDto exchangeTableDto)
        {
            if (!_appDbContext.ExchangeTables.Any(t => t.No == exchangeTableDto.No))
            {
                var exchangeTable = exchangeTableDto.FromDto();

                await _appDbContext.AddAsync(exchangeTable);

                await _appDbContext.AddRangeAsync(exchangeTable.Rates);

                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> CanConnectToDbAsync()
        {
            return await _appDbContext.Database.CanConnectAsync();
        }
    }
}