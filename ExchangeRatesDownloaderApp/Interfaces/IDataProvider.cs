using ExchangeRatesDownloaderApp.Data;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<ExchangeRatesTable>> GetTableAsync(string uri);
    }
}