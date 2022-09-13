using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesDownloaderApp.Data
{
    public static class AppDbInitializer
    {
        public static async Task DoDatabaseMigration(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var _appDbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            var canConnectToDb = await _appDbContext!.Database.CanConnectAsync();
            try
            {
                if (!canConnectToDb)
                {
                    var isDbReadyToWriteData = await _appDbContext.Database.EnsureCreatedAsync();

                    if (!isDbReadyToWriteData)
                    {
                        await _appDbContext.Database.MigrateAsync();
                    }
                }
            }
            catch (Exception)
            {
                await _appDbContext.DisposeAsync();
            }
        }
    }
}