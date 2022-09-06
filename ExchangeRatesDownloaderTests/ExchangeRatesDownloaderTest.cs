using ExchangeRatesDownloaderApp.Controllers;
using ExchangeRatesDownloaderApp.Data;
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
        private IDbDataReader _dataReader;
        private IDbDataWriter _dataWriter;
        private IDataProcessor _dataProcessor;
        private IDataProvider _dataProvider;
        private IExchangeRatesService _service;
        private IConfiguration _configuration;
        private ExchangeRatesController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dataReader = new DbDataReader(_dbContext);
            _dataWriter = new DbDataWriter(_dbContext);
            _dataProvider = new DataProvider();
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _dataProcessor = new DataProcessor(_dataProvider, _configuration, _dataWriter, _dataReader);
            _service = new ExchangeRatesService(_dataProcessor);
            _controller = new ExchangeRatesController(_service);
        }

        [Test, Order(1)]
        public async Task DatabaseShouldBeCreatedOnIfDoesNotExist()
        {
            Assert.That(_dbContext.Database.CanConnect(), Is.True);
        }

        [Test, Order(2)]
        public async Task ShouldBe150RecordsReturnedByControllersIndexAction()
        {
            var actionResult = await _controller.Index() as ViewResult;
            Assert.That(actionResult, Is.TypeOf<ViewResult>());
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Model);
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.ViewName) || actionResult.ViewName == "Index");
            var data = (List<ExchangeRateVM>)actionResult.ViewData.Model;
            Assert.That(data, Has.Count.EqualTo(150));
        }

        [Test, Order(3)]
        public async Task ShouldBe3ExchangeTablesWrittenToDataBase()
        {
            var data = await _dataProcessor.GetDataAsync();
            await _dataWriter.SaveToDbAsync(data);
            var result = await _dataReader.GetAllRatesAsync();
            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test, Order(4)]
        public async Task ShouldBe163RatesTotalWrittenToDataBase()
        {
            var data = await _dataProcessor.GetDataAsync();
            await _dataWriter.SaveToDbAsync(data);
            var result = await _dataReader.GetAllRatesAsync();
            Assert.That(result.SelectMany(x => x.Rates).ToList(), Has.Count.EqualTo(163));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}