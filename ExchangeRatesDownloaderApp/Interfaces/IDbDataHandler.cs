using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbDataHandler
    {
        Task<bool> CanConnectToDbAsync();

        Task<IEnumerable<ExchangeTableDto>> GetTablesAsync();

        Task SaveTableWithRatesToDbAsync(ExchangeTableDto exchangeTableDto);
    }
}