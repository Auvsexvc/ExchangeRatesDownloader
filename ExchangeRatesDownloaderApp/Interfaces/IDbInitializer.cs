namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDbInitializer
    {
        Task EnsureDbCreatedIfPossible();
    }
}