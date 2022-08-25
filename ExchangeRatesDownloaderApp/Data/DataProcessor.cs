using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDataWriter _dataWriter;
        private readonly IDataReader _dataReader;
        private readonly string _nbpTablesApiBaseUrl;
        private readonly string[] _nbpTables;
        private readonly string _outputFormat;

        public DataProcessor(IDataProvider dataProvider, IConfiguration configuration, IDataWriter dataWriter, IDataReader dataReader)
        {
            _dataProvider = dataProvider;
            _dataWriter = dataWriter;
            _dataReader = dataReader;
            _nbpTablesApiBaseUrl = configuration["NbpApi:TablesBaseUrl"];
            _nbpTables = configuration.GetSection("NbpApi:Tables").Get<string[]>();
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

            var objC = data.Where(t => string.Equals(t.Type, "C", StringComparison.CurrentCultureIgnoreCase)).ToList();

            List<ExchangeRateVM> results = new();

            foreach (var table in data.Where(t => !string.Equals(t.Type, "c", StringComparison.CurrentCultureIgnoreCase)))
            {
                foreach (var rate in table.Rates)
                {
                    ExchangeRateVM exchangeRate = new()
                    {
                        Name = rate.Name,
                        Code = rate.Code,
                        Bid = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].Rates.Where(r => r.Code == rate.Code).Select(r => r.Bid).First() : rate.Bid,
                        Ask = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].Rates.Where(r => r.Code == rate.Code).Select(r => r.Ask).First() : rate.Ask,
                        Mid = rate.Mid,
                        No = objC[0].Rates.Any(r => r.Code == rate.Code) ? String.Join("\n", table.No, objC[0].No) : table.No,
                        Type = objC[0].Rates.Any(r => r.Code == rate.Code) ? String.Join("\n", table.Type, objC[0].Type) : table.Type,
                        TradingDate = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].TradingDate : table.TradingDate,
                        EffectiveDate = table.EffectiveDate,
                        EffectiveDateBidAsk = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].EffectiveDate : null
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
            return await _dataProvider.DownloadDataAsync(_nbpTablesApiBaseUrl, _nbpTables, _outputFormat);
        }
    }
}