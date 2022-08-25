using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataWriter
    {
        Task SaveToDb(IEnumerable<ExchangeTable> downloadedTables);
    }
}