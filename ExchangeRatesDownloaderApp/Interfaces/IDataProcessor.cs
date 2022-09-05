using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProcessor
    {
        Task<IEnumerable<ExchangeTable>> GetDataAsync();

        Task<IEnumerable<ExchangeRateVM>> PrepareViewModelAsync();

        Task WriteToDbAsync(IEnumerable<ExchangeTable> exchangeTables);
    }
}