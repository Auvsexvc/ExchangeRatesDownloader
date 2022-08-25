using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<ExchangeRateVM>> GetExchangeRatesAsync();

        Task ImportExchangeRatesAsync();
    }
}