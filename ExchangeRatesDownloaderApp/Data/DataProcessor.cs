using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using System.Net;
using System.Linq;

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

        private IEnumerable<ExchangeRateVM> PrepareViewModel(IEnumerable<ExchangeTable> exchangeTables)
        {
            var bidAskTables = exchangeTables
                .Where(t => _nbpTablesBidAsk
                    .Select(x => x.ToLower())
                    .Contains(t.Type.ToLower()))
                .ToList();

            var midTables = exchangeTables
                .Where(t => _nbpTablesMid
                    .Select(x => x.ToLower())
                    .Contains(t.Type.ToLower()));

            List<ExchangeRateVM> exchangeRates = new();

            foreach (var (midTable, rate) in midTables.SelectMany(midTable => midTable.Rates.Select(rate => (midTable, rate))))
            {
                (int index, bool currencyHasBidAsk) = Enumerable
                                    .Range(0, bidAskTables.Count)
                                    .Select(x => (x, hasBidAsk: bidAskTables[x].Rates.Any(r => r.Code == rate.Code)))
                                    .FirstOrDefault(x => x.hasBidAsk);

                ExchangeRateVM exchangeRate = new()
                {
                    Name = rate.Name,
                    Code = rate.Code,
                    Bid = currencyHasBidAsk ? bidAskTables[index].Rates.Where(r => r.Code == rate.Code).Select(r => r.Bid).First() : rate.Bid,
                    Ask = currencyHasBidAsk ? bidAskTables[index].Rates.Where(r => r.Code == rate.Code).Select(r => r.Ask).First() : rate.Ask,
                    Mid = rate.Mid,
                    No = currencyHasBidAsk ? String.Join("\n", midTable.No, bidAskTables[index].No) : midTable.No,
                    Type = currencyHasBidAsk ? String.Join("\n", midTable.Type, bidAskTables[index].Type) : midTable.Type,
                    TradingDate = currencyHasBidAsk ? bidAskTables[index].TradingDate : midTable.TradingDate,
                    EffectiveDate = midTable.EffectiveDate,
                    EffectiveDateBidAsk = currencyHasBidAsk ? bidAskTables[index].EffectiveDate : null
                };
                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }

        private async Task<IEnumerable<ExchangeTable>> ReadFromDbAsync() => await _dataReader.GetAllRatesAsync();

        private async Task<IEnumerable<ExchangeTable>> ReadFromProviderAsync() => await GetDataFromProviderAsync();

        private string MakeUri(string tableType) => _nbpTablesApiBaseUrl + tableType + "?" + _outputFormat;
    }
}