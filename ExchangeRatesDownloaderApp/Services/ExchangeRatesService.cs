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
            return await _dataProcessor.PrepareViewModelAsync();
        }

        public async Task ImportExchangeRatesAsync()
        {
            try
            {
                await _dataProcessor.WriteToDbAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}