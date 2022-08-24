using ExchangeRatesDownloaderApp.Interfaces;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataProvider _dataProvider;

        public DataProcessor(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IEnumerable<ExchangeRate>> Process()
        {
            var objA = await _dataProvider.GetTableAsync("http://api.nbp.pl/api/exchangerates/tables/a?format=json");
            var objB = await _dataProvider.GetTableAsync("http://api.nbp.pl/api/exchangerates/tables/b?format=json");
            var objC = await _dataProvider.GetTableAsync("http://api.nbp.pl/api/exchangerates/tables/c?format=json");

            List<ExchangeRate> results = new List<ExchangeRate>();

            foreach (var table in objA.Concat(objB))
            {
                foreach (var rate in table.Rates)
                {
                    ExchangeRate exchangeRate = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = rate.Name,
                        Code = rate.Code,
                        Bid = objC.First().Rates.Any(r => r.Code == rate.Code) ? objC.First().Rates.Where(r => r.Code == rate.Code).Select(r => r.Bid).First() : rate.Bid,
                        Ask = objC.First().Rates.Any(r => r.Code == rate.Code) ? objC.First().Rates.Where(r => r.Code == rate.Code).Select(r => r.Ask).First() : rate.Ask,
                        Mid = rate.Mid,
                        No = table.No,
                        Type = table.Type,
                        TradingDate = table.TradingDate,
                        EffectiveDate = table.EffectiveDate
                    };
                    results.Add(exchangeRate);
                }
            }
            return results;
        }
    }
}