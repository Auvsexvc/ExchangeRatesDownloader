using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProcessor
    {
        Task WriteToDbAsync();
        Task<IEnumerable<ExchangeRateVM>> ReadFromDbAsync();
        Task<IEnumerable<ExchangeRateVM>> ReadFromProviderAsync();
    }
}