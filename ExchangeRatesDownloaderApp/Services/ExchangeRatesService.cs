using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly IDataProcessor _dataProcessor;

        public ExchangeRatesService(IDataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        public async Task<IEnumerable<ExchangeRateVM>> GetExchangeRatesAsync()
        {
            return await _dataProcessor.GetViewDataFromAvailableSourceAsync();
        }

        public async Task ImportExchangeRatesAsync()
        {
            var data = await _dataProcessor.GetDataFromProviderAsync();
            await _dataProcessor.WriteToDbAsync(data);
        }
    }
}