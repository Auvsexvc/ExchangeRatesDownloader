using ExchangeRatesDownloaderApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRatesDownloaderApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _homeService.ImportCurrentExchangesRates();


            return View(data.OrderBy(x=>x.Name));
        }
    }
}