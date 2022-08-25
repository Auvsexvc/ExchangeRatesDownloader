using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataReader
    {
        Task<IEnumerable<ExchangeRateVM>> GetAllRatesAsync();
    }
}