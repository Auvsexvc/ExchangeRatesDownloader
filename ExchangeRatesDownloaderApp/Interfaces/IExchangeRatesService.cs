using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IExchangeRatesService
    {
        Task<(IEnumerable<ExchangeRateVM>, string, string)> GetExchangeRatesAsync();
    }
}