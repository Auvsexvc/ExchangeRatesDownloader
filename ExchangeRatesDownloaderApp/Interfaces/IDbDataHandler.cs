using ExchangeRatesDownloaderApp.Entities;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbDataHandler
    {
        Task<bool> CanConnectToDbAsync();

        Task<IEnumerable<ExchangeTable>> GetRecentAsync();

        Task SaveToDbAsync(ExchangeTable exchangeTable);
    }
}