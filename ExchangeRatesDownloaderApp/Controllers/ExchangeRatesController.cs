using ExchangeRatesDownloaderApp.Helper;
using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace ExchangeRatesDownloaderApp.Controllers
{
    public class ExchangeRatesController : Controller
    {
        private readonly IExchangeRatesService _exchangeRatesService;

        public ExchangeRatesController(IExchangeRatesService exchangeRatesService)
        {
            _exchangeRatesService = exchangeRatesService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await _exchangeRatesService.ImportExchangeRatesAsync();
                ViewBag.WriteToDb = ControllerMessages.MSG_GETDATASUCCESS;
            }
            catch (SqlException ex)
            {
                ViewBag.WriteToDb = String.Format(ControllerMessages.MSG_DBWRITESQLFAIL, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.WriteToDb = String.Format(ControllerMessages.MSG_DBWRITEHTTPFAIL, ex.Message);
            }

            try
            {
                var data = await _exchangeRatesService.GetExchangeRatesAsync();
                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.ShowData = String.Format(ControllerMessages.MSG_GETDATAFAIL, ex.Message);
                return View(Enumerable.Empty<ExchangeRateVM>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}