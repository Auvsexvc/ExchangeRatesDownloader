using ExchangeRatesDownloaderApp.Controllers;
using ExchangeRatesDownloaderApp.Data;
using ExchangeRatesDownloaderApp.Extensions;
using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using ExchangeRatesDownloaderApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExchangeRatesDownloaderTests
{
    public class ExchangeRatesDownloaderTest
    {
        private static readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ExchangeRatesDbTest")
            .Options;

        private AppDbContext _dbContext;
        private IDbInitializer _dbInitializer;
        private IDbDataHandler _dataHandler;
        private IDataProcessor _dataProcessor;
        private IExternalProvider _dataProvider;
        private IExchangeRatesService _service;
        private IConfiguration _configuration;
        private ExchangeRatesController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbInitializer = new DbInitializer(_dbContext);
            _dataHandler = new DbDataHandler(_dbContext);
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _dataProvider = new ExternalProvider(_configuration);
            _dataProcessor = new DataProcessor(_dataProvider, _dataHandler);
            _service = new ExchangeRatesService(_dataProcessor);
            _controller = new ExchangeRatesController(_service);
        }

        [Test, Order(1)]
        public async Task DatabaseShouldBeCreatedOnIfDoesNotExist()
        {
            await _dbInitializer.EnsureDbCreatedIfPossible();
            Assert.That(await _dbContext.Database.EnsureCreatedAsync(), Is.False);
        }

        [Test, Order(2)]
        public async Task ShouldBe150RecordsReturnedByControllersIndexAction()
        {
            var actionResult = await _controller.Index() as ViewResult;
            Assert.That(actionResult, Is.TypeOf<ViewResult>());
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.Model, Is.Not.Null);
                Assert.That(string.IsNullOrEmpty(actionResult.ViewName) || actionResult.ViewName == "Index", Is.True);
            });
            var data = (IEnumerable<ExchangeRateVM>?)actionResult.ViewData.Model;
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Count(), Is.EqualTo(150));
        }

        [Test, Order(3)]
        public async Task ShouldBe3ExchangeTablesWrittenToDataBase()
        {
            var data = await _dataProvider.GetDtosAsync();
            foreach (var item in data)
            {
                await _dataHandler.SaveToDbAsync(item.FromDto());
            }
            var result = await _dataHandler.GetRecentAsync();
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test, Order(4)]
        public async Task ShouldBe163RatesTotalWrittenToDataBase()
        {
            var data = await _dataProvider.GetDtosAsync();
            foreach (var item in data)
            {
                await _dataHandler.SaveToDbAsync(item.FromDto());
            }
            var result = await _dataHandler.GetRecentAsync();
            Assert.That(result.Sum(x => x.Rates.Count), Is.EqualTo(163));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}