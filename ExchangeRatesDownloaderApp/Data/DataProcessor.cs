using ExchangeRatesDownloaderApp.Extensions;
using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IExternalProvider _externalProvider;
        private readonly IDbDataHandler _dbDataHandler;

        public DataProcessor(IExternalProvider externalProvider, IDbDataHandler dbDataHandler)
        {
            _externalProvider = externalProvider;
            _dbDataHandler = dbDataHandler;
        }

        public async Task<IEnumerable<ExchangeRateVM>> GetRatesAsync()
        {
            if (await _dbDataHandler.CanConnectToDbAsync())
            {
                var data = (await _dbDataHandler.GetRecentAsync()).Select(x => x.ToDto());

                return data.ConvertToVMs();
            }

            return (await _externalProvider.GetDtosAsync()).ConvertToVMs();
        }

        public async Task ImportToDbAsync()
        {
            var exchangeTables = await _externalProvider.GetDtosAsync();
            var dbData = await _dbDataHandler.GetRecentAsync();

            foreach (var exchangeTable in exchangeTables)
            {
                if (!dbData.Any(t => t.No == exchangeTable.No))
                {
                    var data = exchangeTable.FromDto();
                    await _dbDataHandler.SaveToDbAsync(data);
                }
            }
        }
    }
}