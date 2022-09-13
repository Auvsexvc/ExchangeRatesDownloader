using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProcessor
    {
        Task<IEnumerable<ExchangeRateVM>> GetRates();

        Task ImportExchangeRatesAsync();
    }
}