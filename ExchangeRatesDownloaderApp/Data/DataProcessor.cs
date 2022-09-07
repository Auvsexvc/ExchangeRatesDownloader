using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using System.Net;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDbDataWriter _dataWriter;
        private readonly IDbDataReader _dataReader;
        private readonly string _nbpTablesApiBaseUrl;
        private readonly string[] _nbpTablesMid;
        private readonly string[] _nbpTablesBidAsk;
        private readonly string _outputFormat;

        public DataProcessor(IDataProvider dataProvider, IConfiguration configuration, IDbDataWriter dataWriter, IDbDataReader dataReader)
        {
            _dataProvider = dataProvider;
            _dataWriter = dataWriter;
            _dataReader = dataReader;
            _nbpTablesApiBaseUrl = configuration["NbpApi:TablesBaseUrl"];
            _nbpTablesMid = configuration.GetSection("NbpApi:Tables:Mid").Get<string[]>();
            _nbpTablesBidAsk = configuration.GetSection("NbpApi:Tables:BidAsk").Get<string[]>();
            _outputFormat = configuration["NbpApi:OutputFormat"];
        }

        public async Task WriteToDbAsync(IEnumerable<ExchangeTable> exchangeTables)
        {
            await _dataWriter.SaveToDbAsync(exchangeTables);
        }

        public async Task<IEnumerable<ExchangeRateVM>> GetViewDataFromAvailableSourceAsync()
        {
            if (await _dataReader.CanConnectToDbAsync())
            {
                return PrepareViewModel(await ReadFromDbAsync());
            }

            return PrepareViewModel(await ReadFromProviderAsync());
        }

        public async Task<IEnumerable<ExchangeTable>> GetDataFromProviderAsync()
        {
            List<HttpResponseMessage> httpResponses = new();

            foreach (var tableType in _nbpTablesMid.Concat(_nbpTablesBidAsk).ToArray())
            {
                var httpResponse = await _dataProvider.GetResponseAsync(MakeUri(tableType));
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    httpResponses.Add(httpResponse);
                }
            }

            return await _dataProvider.DeserializeAsync(httpResponses);
        }

        private IEnumerable<ExchangeRateVM> PrepareViewModel(IEnumerable<ExchangeTable> data)
        {
            var objC = data.Where(t => _nbpTablesBidAsk.Select(x => x.ToLower()).Contains(t.Type.ToLower())).ToList();

            List<ExchangeRateVM> exchangeRates = new();

            foreach (var table in data.Where(t => _nbpTablesMid.Select(x => x.ToLower()).Contains(t.Type.ToLower())))
            {
                foreach (var rate in table.Rates)
                {
                    (int index, bool hasBidAsk) = Enumerable
                        .Range(0, objC.Count)
                        .Select(x => (x, hasBidAsk: objC[x].Rates.Any(r => r.Code == rate.Code)))
                        .FirstOrDefault(x => x.hasBidAsk);

                    ExchangeRateVM exchangeRate = new()
                    {
                        Name = rate.Name,
                        Code = rate.Code,
                        Bid = hasBidAsk ? objC[index].Rates.Where(r => r.Code == rate.Code).Select(r => r.Bid).First() : rate.Bid,
                        Ask = hasBidAsk ? objC[index].Rates.Where(r => r.Code == rate.Code).Select(r => r.Ask).First() : rate.Ask,
                        Mid = rate.Mid,
                        No = hasBidAsk ? String.Join("\n", table.No, objC[index].No) : table.No,
                        Type = hasBidAsk ? String.Join("\n", table.Type, objC[index].Type) : table.Type,
                        TradingDate = hasBidAsk ? objC[index].TradingDate : table.TradingDate,
                        EffectiveDate = table.EffectiveDate,
                        EffectiveDateBidAsk = hasBidAsk ? objC[index].EffectiveDate : null
                    };
                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }

        private async Task<IEnumerable<ExchangeTable>> ReadFromDbAsync() => await _dataReader.GetAllRatesAsync();

        private async Task<IEnumerable<ExchangeTable>> ReadFromProviderAsync() => await GetDataFromProviderAsync();

        private string MakeUri(string tableType) => _nbpTablesApiBaseUrl + tableType + "?" + _outputFormat;
    }
}