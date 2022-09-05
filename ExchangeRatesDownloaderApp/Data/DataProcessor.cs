using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;

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

        public async Task WriteToDbAsync()
        {
            var data = await GetDataAsync();
            await _dataWriter.SaveToDb(data);
        }

        public async Task<IEnumerable<ExchangeRateVM>> PrepareViewModelAsync()
        {
            List<ExchangeTable> data;

            if (await _dataReader.CanConnectToDb())
            {
                data = await ReadFromDbAsync();
            }
            else
            {
                data = await ReadFromProviderAsync();
            }

            var objC = data.Where(t => _nbpTablesBidAsk.Select(x => x.ToLower()).Contains(t.Type.ToLower())).ToList();

            List<ExchangeRateVM> results = new();

            foreach (var table in data.Where(t => _nbpTablesMid.Select(x => x.ToLower()).Contains(t.Type.ToLower())))
            {
                foreach (var rate in table.Rates)
                {
                    var isCurrencyHavingBidAsk = false;
                    var idx = 0;
                    for (int i = 0; i < objC.Count; i++)
                    {
                        isCurrencyHavingBidAsk = objC[i].Rates.Any(r => r.Code == rate.Code);
                        if (isCurrencyHavingBidAsk)
                        {
                            idx = i;
                            break;
                        }
                    }

                    ExchangeRateVM exchangeRate = new()
                    {
                        Name = rate.Name,
                        Code = rate.Code,
                        Bid = isCurrencyHavingBidAsk ? objC[idx].Rates.Where(r => r.Code == rate.Code).Select(r => r.Bid).First() : rate.Bid,
                        Ask = isCurrencyHavingBidAsk ? objC[idx].Rates.Where(r => r.Code == rate.Code).Select(r => r.Ask).First() : rate.Ask,
                        Mid = rate.Mid,
                        No = isCurrencyHavingBidAsk ? String.Join("\n", table.No, objC[idx].No) : table.No,
                        Type = isCurrencyHavingBidAsk ? String.Join("\n", table.Type, objC[idx].Type) : table.Type,
                        TradingDate = isCurrencyHavingBidAsk ? objC[idx].TradingDate : table.TradingDate,
                        EffectiveDate = table.EffectiveDate,
                        EffectiveDateBidAsk = isCurrencyHavingBidAsk ? objC[idx].EffectiveDate : null
                    };
                    results.Add(exchangeRate);
                }
            }
            return results;
        }

        private async Task<List<ExchangeTable>> ReadFromDbAsync()
        {
            return await _dataReader.GetAllRatesAsync();
        }

        private async Task<List<ExchangeTable>> ReadFromProviderAsync()
        {
            return new List<ExchangeTable>(await GetDataAsync());
        }

        private async Task<IEnumerable<ExchangeTable>> GetDataAsync()
        {
            return await _dataProvider.DownloadDataAsync(_nbpTablesApiBaseUrl, _nbpTablesMid.Concat(_nbpTablesBidAsk).ToArray(), _outputFormat);
        }
    }
}