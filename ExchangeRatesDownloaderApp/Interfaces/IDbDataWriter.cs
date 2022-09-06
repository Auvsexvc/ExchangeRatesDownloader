using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbDataWriter
    {
        Task SaveToDbAsync(IEnumerable<ExchangeTable> downloadedTables);
    }
}