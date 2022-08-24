namespace ExchangeRatesDownloaderApp.Data
{
    public interface IDataProcessor
    {
        Task<IEnumerable<ExchangeRate>> Process();
    }
}