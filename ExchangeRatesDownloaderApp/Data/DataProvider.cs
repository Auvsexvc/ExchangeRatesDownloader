using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Newtonsoft.Json;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProvider : IDataProvider
    {
        public async Task<HttpResponseMessage> GetResponseAsync(string uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            return response;
        }

        public async Task<IEnumerable<ExchangeTable>> Deserialize(IEnumerable<HttpResponseMessage> httpResponses)
        {
            var exchangeTables = new List<ExchangeTable>();

            foreach (var response in httpResponses)
            {
                var content = await response.Content.ReadAsStringAsync();
                exchangeTables.Add(await Task.Run(() => JsonConvert.DeserializeObject<List<ExchangeTable>>(content)[0]));
            }

            return exchangeTables;
        }
    }
}