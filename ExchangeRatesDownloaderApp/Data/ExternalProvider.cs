using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Newtonsoft.Json;
using System.Net;

namespace ExchangeRatesDownloaderApp.Data
{
    public class ExternalProvider : IExternalProvider
    {
        private readonly string _nbpTablesApiBaseUrl;
        private readonly string _outputFormat;
        private readonly string[] _nbpTablesMid;
        private readonly string[] _nbpTablesBidAsk;

        public ExternalProvider(IConfiguration configuration)
        {
            _nbpTablesApiBaseUrl = configuration["NbpApi:TablesBaseUrl"];
            _outputFormat = configuration["NbpApi:OutputFormat"];
            _nbpTablesMid = configuration.GetSection("NbpApi:Tables:Mid").Get<string[]>();
            _nbpTablesBidAsk = configuration.GetSection("NbpApi:Tables:BidAsk").Get<string[]>();
        }

        public async Task<IEnumerable<ExchangeTableDto>> GetDtosAsync()
        {
            List<ExchangeTableDto> exchangeTablesDtos = new();

            foreach (var tableType in _nbpTablesMid.Concat(_nbpTablesBidAsk).ToArray())
            {
                var httpResponse = await GetRemoteDataAsync(MakeUri(tableType));
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    exchangeTablesDtos.Add(await DeserializeAsync(httpResponse));
                }
            }

            return exchangeTablesDtos;
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