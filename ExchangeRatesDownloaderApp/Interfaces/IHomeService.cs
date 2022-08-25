using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<ExchangeRate>> ImportCurrentExchangesRates();
    }
}