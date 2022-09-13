using ExchangeRatesDownloaderApp.Data;
using ExchangeRatesDownloaderApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatesDownloaderApp.Services
{
    public class StartupService : IHostedService
    {
        private readonly IServiceProvider _serviceScopeFactory;

        public StartupService(IServiceProvider serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _appDbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();

            await _appDbInitializer!.EnsureDbCreatedIfPossible();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}