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

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDataProvider, DataProvider>();
builder.Services.AddScoped<IDataWriter, DataWriter>();
builder.Services.AddScoped<IDataReader, DataReader>();
builder.Services.AddScoped<IDataProcessor, DataProcessor>();
builder.Services.AddScoped<IHomeService, HomeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();