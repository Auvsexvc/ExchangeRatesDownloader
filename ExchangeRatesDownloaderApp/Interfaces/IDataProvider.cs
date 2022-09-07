using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<ExchangeTable>> DeserializeAsync(IEnumerable<HttpResponseMessage> httpResponses);

        Task<HttpResponseMessage> GetResponseAsync(string uri);
    }
}