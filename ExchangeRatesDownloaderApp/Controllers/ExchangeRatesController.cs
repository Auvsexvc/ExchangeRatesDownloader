using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Microsoft.AspNetCore.Mvc;
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
            var data = await _exchangeRatesService.GetExchangeRatesViewAsync();

            ViewBag.WriteToDb = data.Item2;
            ViewBag.ShowData = data.Item3;

            return View(data.Item1);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}