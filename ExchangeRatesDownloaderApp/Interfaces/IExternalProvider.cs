using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IExternalProvider
    {
        Task<IEnumerable<ExchangeTableDto>> GetDtosAsync();
    }
}