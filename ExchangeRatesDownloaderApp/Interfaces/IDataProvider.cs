using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProvider
    {
        Task<List<ExchangeTable>> DownloadDataAsync(string baseUri, string[] tables, string outputFormat);
    }
}