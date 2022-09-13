using ExchangeRatesDownloaderApp.Data;
using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Middleware;
using ExchangeRatesDownloaderApp.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDataProvider, DataProvider>();
builder.Services.AddScoped<IDbDataWriter, DbDataWriter>();
builder.Services.AddScoped<IDbDataReader, DbDataReader>();
builder.Services.AddScoped<IDataProcessor, DataProcessor>();
builder.Services.AddScoped<IExchangeRatesService, ExchangeRatesService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ExchangeRates/Error");
    app.UseHsts();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ExchangeRates}/{action=Index}");

await AppDbInitializer.DoDatabaseMigration(app);

app.Run();