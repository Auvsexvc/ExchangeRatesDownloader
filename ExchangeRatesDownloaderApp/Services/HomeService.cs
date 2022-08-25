using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Services
{
    public class HomeService : IHomeService
    {
        private readonly IDataProcessor _dataProcessor;

        public HomeService(IDataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        public async Task<IEnumerable<ExchangeRateVM>> GetExchangeRatesAsync()
        {
            return await _dataProcessor.PrepareViewModelAsync();
        }

        public async Task ImportExchangeRatesAsync()
        {
            await _dataProcessor.WriteToDbAsync();
        }
    }
}