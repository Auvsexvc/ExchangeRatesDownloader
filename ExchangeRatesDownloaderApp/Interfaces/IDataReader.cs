using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataReader
    {
        Task<List<ExchangeTable>> GetAllRatesAsync();
        Task<bool> CanConnectToDb();
    }
}