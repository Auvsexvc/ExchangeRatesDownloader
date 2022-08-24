using ExchangeRatesDownloaderApp.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProvider : IDataProvider
    {
        public async Task<IEnumerable<ExchangeRatesTable>> GetTableAsync(string uri)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(uri);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ExchangeRatesTable>>(content);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                throw;
            }
        }
    }
}