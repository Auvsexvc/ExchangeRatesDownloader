using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataReader : IDataReader
    {
        private readonly AppDbContext _appDbContext;

        public DataReader(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ExchangeTable>> GetAllRatesAsync()
        {
            List<ExchangeTable> data = new();

            if (await _appDbContext.Database.CanConnectAsync())
            {
                data = await _appDbContext
                    .ExchangeTables
                    .Include(x => x.Rates)
                    .GroupBy(x => x.Type)
                    .Select(x => x.OrderByDescending(x => x.EffectiveDate).First())
                    .ToListAsync();
            }

            return data;
        }

        public async Task<bool> CanConnectToDb()
        {
            return await _appDbContext.Database.CanConnectAsync();
        }
    }
}