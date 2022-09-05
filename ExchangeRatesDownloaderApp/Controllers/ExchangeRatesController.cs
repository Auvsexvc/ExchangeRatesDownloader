using ExchangeRatesDownloaderApp.Interfaces;
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
            ViewBag.ApiState = "No errors found.";
            ViewBag.DbState = "No errors found.";
            try
            {
                await _homeService.ImportExchangeRatesAsync();
            }
            catch (SqlException ex)
            {
                ViewBag.DBState = $"{ex.Message} - Showing the most recent exchange rates from external API";
            }
            catch (Exception ex)
            {
                ViewBag.ApiState = $"{ex.Message} - Showing the most recent exchange rates stored in database";
            }

            var data = await _homeService.GetExchangeRatesAsync();

            return View(data);
        }
    }
}