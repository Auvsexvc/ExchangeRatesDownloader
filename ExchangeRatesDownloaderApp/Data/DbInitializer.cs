using ExchangeRatesDownloaderApp.Interfaces;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _appDbContext;

        public DbInitializer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task EnsureDbCreatedIfPossible()
        {
            try
            {
                await _appDbContext!.Database.CanConnectAsync();
                await _appDbContext.Database.EnsureCreatedAsync();
            }
            catch
            {
                await _appDbContext.DisposeAsync();
            }
        }
    }
}