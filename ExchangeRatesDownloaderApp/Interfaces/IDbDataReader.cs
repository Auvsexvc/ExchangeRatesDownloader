using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbDataReader
    {
        Task<bool> CanConnectToDbAsync();

        Task<List<ExchangeTable>> GetAllRatesAsync();
    }
}