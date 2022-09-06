using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProvider
    {
        Task<HttpResponseMessage> GetResponseAsync(string uri);

        Task<IEnumerable<ExchangeTable>> Deserialize(IEnumerable<HttpResponseMessage> httpResponses);
    }
}