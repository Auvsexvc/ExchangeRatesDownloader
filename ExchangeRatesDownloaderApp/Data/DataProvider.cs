using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProvider : IDataProvider
    {
        public async Task<IEnumerable<ExchangeTable>> DownloadDataAsync(string baseUri, string[] tables, string outputFormat)
        {
            List<ExchangeTable> tableList = new();

            foreach (var item in tables)
            {
                string uri = baseUri + item + "?" + outputFormat;
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(uri);

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    tableList.Add(JsonConvert.DeserializeObject<List<ExchangeTable>>(content)[0]);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    throw;
                }
            }

            return tableList;
        }
    }
}