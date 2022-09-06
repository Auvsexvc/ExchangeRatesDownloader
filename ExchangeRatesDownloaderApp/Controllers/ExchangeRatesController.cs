﻿using ExchangeRatesDownloaderApp.Helper;
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
            try
            {
                await _homeService.ImportExchangeRatesAsync();
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
                var data = await _homeService.GetExchangeRatesAsync();
                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.ShowData = String.Format(ControllerMessages.MSG_GETDATAFAIL, ex.Message);
                return View(Enumerable.Empty<ExchangeRateVM>());
            }
        }
    }
}