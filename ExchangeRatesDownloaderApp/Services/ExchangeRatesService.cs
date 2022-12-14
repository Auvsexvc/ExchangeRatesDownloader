using ExchangeRatesDownloaderApp.Helper;
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

        public async Task<(IEnumerable<ExchangeRateVM>, string, string)> GetExchangeRatesAsync()
        {
            string dbMsg = string.Empty;
            try
            {
                await _dataProcessor.ImportToDbAsync();
            }
            catch (Exception ex)
            {
                dbMsg = String.Format(Messages.MSG_DBWRITEFAIL, ex.Message);
            }

            IEnumerable<ExchangeRateVM> data = Enumerable.Empty<ExchangeRateVM>();
            string apiMsg = string.Empty;

            try
            {
                data = await _dataProcessor.GetRatesAsync();
            }
            catch (Exception ex)
            {
                apiMsg = String.Format(Messages.MSG_GETDATAFAIL, ex.Message);
            }

            return (data, dbMsg, apiMsg);
        }
    }
}