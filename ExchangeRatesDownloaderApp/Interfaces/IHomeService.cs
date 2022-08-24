using ExchangeRatesDownloaderApp.Data;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<ExchangeRate>> ImportCurrentExchangesRates();
    }
}