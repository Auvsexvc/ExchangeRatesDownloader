using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataWriter : IDataWriter
    {
        private readonly AppDbContext _appDbContext;

        public DataWriter(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task SaveToDb(IEnumerable<ExchangeTable> downloadedTables)
        {
            var canConnectToDb = await _appDbContext.Database.CanConnectAsync();
            if (!canConnectToDb)
            {
                var isDbReadyToWriteData = await _appDbContext.Database.EnsureCreatedAsync();

                if (!isDbReadyToWriteData)
                {
                    await _appDbContext.Database.MigrateAsync();
                }
            }

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