using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataProvider _dataProvider;
        private readonly string _nbpTablesApiBaseUrl;

        public DataProcessor(IDataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _nbpTablesApiBaseUrl = configuration["NbpApi:TablesBaseUrl"];
        }

        public async Task<IEnumerable<ExchangeRate>> Process()
        {
            var objA = await _dataProvider.GetTableAsync(_nbpTablesApiBaseUrl + "a?format=json");
            var objB = await _dataProvider.GetTableAsync(_nbpTablesApiBaseUrl + "b?format=json");
            var objC = await _dataProvider.GetTableAsync(_nbpTablesApiBaseUrl + "c?format=json");

            List<ExchangeRate> results = new();

            foreach (var table in objA.Concat(objB))
            {
                foreach (var rate in table.Rates)
                {
                    ExchangeRate exchangeRate = new()
                    {
                        Name = rate.Name,
                        Code = rate.Code,
                        Bid = objC.First().Rates.Any(r => r.Code == rate.Code) ? objC.First().Rates.Where(r => r.Code == rate.Code).Select(r => r.Bid).First() : rate.Bid,
                        Ask = objC.First().Rates.Any(r => r.Code == rate.Code) ? objC.First().Rates.Where(r => r.Code == rate.Code).Select(r => r.Ask).First() : rate.Ask,
                        Mid = rate.Mid,
                        No = objC.First().Rates.Any(r => r.Code == rate.Code) ? String.Join("\n", table.No, objC.First().No) : table.No,
                        Type = objC.First().Rates.Any(r => r.Code == rate.Code) ? String.Join("\n", table.Type, objC.First().Type) : table.Type,
                        TradingDate = objC.First().Rates.Any(r => r.Code == rate.Code) ? objC.First().TradingDate : table.TradingDate,
                        EffectiveDate = table.EffectiveDate
                    };
                    results.Add(exchangeRate);
                }
            }
            return results;
        }
    }
}