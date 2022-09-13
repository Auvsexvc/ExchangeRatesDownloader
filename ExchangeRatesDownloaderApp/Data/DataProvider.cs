using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Newtonsoft.Json;
using System.Net;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly string _nbpTablesApiBaseUrl;
        private readonly string _outputFormat;

        public string[] NbpTablesMid { get; }

        public string[] NbpTablesBidAsk { get; }

        public DataProvider(IConfiguration configuration)
        {
            _nbpTablesApiBaseUrl = configuration["NbpApi:TablesBaseUrl"];
            _outputFormat = configuration["NbpApi:OutputFormat"];
            NbpTablesMid = configuration.GetSection("NbpApi:Tables:Mid").Get<string[]>();
            NbpTablesBidAsk = configuration.GetSection("NbpApi:Tables:BidAsk").Get<string[]>();
        }

        public async Task<IEnumerable<ExchangeTableDto>> GetTablesAsync()
        {
            List<ExchangeTableDto> httpResponses = new();

            foreach (var tableType in NbpTablesMid.Concat(NbpTablesBidAsk).ToArray())
            {
                var httpResponse = await GetRemoteDataAsync(MakeUri(tableType));
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    httpResponses.Add(await DeserializeAsync(httpResponse));
                }
            }

            return httpResponses;
        }

        private static async Task<HttpResponseMessage> GetRemoteDataAsync(string uri)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            return response;
        }

        private static async Task<ExchangeTableDto> DeserializeAsync(HttpResponseMessage httpResponses)
        {
            var content = await httpResponses.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<List<ExchangeTableDto>>(content)![0]);
        }

        private string MakeUri(string tableType) => _nbpTablesApiBaseUrl + tableType + "?" + _outputFormat;
    }
}