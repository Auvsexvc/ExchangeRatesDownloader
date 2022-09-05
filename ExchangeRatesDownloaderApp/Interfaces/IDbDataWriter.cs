using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbDataWriter
    {
        Task SaveToDb(IEnumerable<ExchangeTable> downloadedTables);
    }
}