using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ExchangeRatesDownloaderApp.Controllers
{
    public class ExchangeRatesController : Controller
    {
        private readonly IExchangeRatesService _homeService;

        public ExchangeRatesController(IExchangeRatesService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.WriteToDb = "Db successfully updated";
            ViewBag.ShowData = String.Empty;
            try
            {
                await _homeService.ImportExchangeRatesAsync();
            }
            catch (SqlException ex)
            {
                ViewBag.WriteToDb = $"Writing to db failed. - {ex.Message}";
            }
            catch (HttpRequestException ex)
            {
                ViewBag.WriteToDb = $"Writing to db failed. Reading data from remote API not possible - {ex.Message}";
            }

            var data = Enumerable.Empty<ExchangeRateVM>();
            try
            {
                data = await _homeService.GetExchangeRatesAsync();
            }
            catch (Exception ex)
            {
                ViewBag.ShowData = $"Data cannot be shown - {ex.Message}";
            }

            return View(data);
        }
    }
}