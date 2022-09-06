using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<ExchangeTable>> Deserialize(IEnumerable<HttpResponseMessage> httpResponses);

        Task<HttpResponseMessage> GetResponseAsync(string uri);
    }
}