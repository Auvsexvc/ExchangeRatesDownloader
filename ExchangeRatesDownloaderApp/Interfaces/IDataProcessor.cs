using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProcessor
    {
        Task<IEnumerable<ExchangeRateVM>> GetRatesAsync();

        Task ImportToDbAsync();
    }
}