using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<ExchangeTable>> DownloadDataAsync(string baseUri, string[] tables, string outputFormat);
    }
}