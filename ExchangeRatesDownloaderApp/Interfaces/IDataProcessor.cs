using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProcessor
    {
        Task<IEnumerable<ExchangeTable>> GetDataFromProviderAsync();

        Task<IEnumerable<ExchangeRateVM>> GetViewDataFromAvailableSourceAsync();

        Task WriteToDbAsync(IEnumerable<ExchangeTable> exchangeTables);
    }
}