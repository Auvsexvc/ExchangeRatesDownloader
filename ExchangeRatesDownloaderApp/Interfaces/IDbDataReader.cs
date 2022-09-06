using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbDataReader
    {
        Task<bool> CanConnectToDb();

        Task<List<ExchangeTable>> GetAllRatesAsync();
    }
}