using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDbDataHandler _dataHandler;

        public DataProcessor(IDataProvider dataProvider, IDbDataHandler dataHandler)
        {
            _dataProvider = dataProvider;
            _dataHandler = dataHandler;
        }

        public async Task<IEnumerable<ExchangeRateVM>> GetRates()
        {
            if (await _dataHandler.CanConnectToDbAsync())
            {
                return PrepareViewModel(await _dataHandler.GetTablesAsync());
            }

            return PrepareViewModel(await _dataProvider.GetTablesAsync());
        }

        public async Task ImportExchangeRatesAsync()
        {
            var exchangeTables = await _dataProvider.GetTablesAsync();

            foreach (var exchangeTable in exchangeTables)
            {
                await _dataHandler.SaveTableWithRatesToDbAsync(exchangeTable);
            }
        }

        private IEnumerable<ExchangeRateVM> PrepareViewModel(IEnumerable<ExchangeTableDto> exchangeTables)
        {
            var bidAskTables = exchangeTables
                .Where(t => _dataProvider.NbpTablesBidAsk
                    .Select(x => x.ToLower())
                    .Contains(t.Type.ToLower()))
                .ToList();

            var midTables = exchangeTables
                .Where(t => _dataProvider.NbpTablesMid
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
    }
}