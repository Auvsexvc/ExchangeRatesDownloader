using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbDataReader
    {
        Task<List<ExchangeTable>> GetAllRatesAsync();
        Task<bool> CanConnectToDb();
    }
}