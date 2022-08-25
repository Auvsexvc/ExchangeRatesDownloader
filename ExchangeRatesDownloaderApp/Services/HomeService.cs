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

        public async Task<IEnumerable<ExchangeRate>> ImportCurrentExchangesRates()
        {
            var result = await _dataProcessor.Process();

            return result;
        }
    }
}