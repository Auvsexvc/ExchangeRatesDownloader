using ExchangeRatesDownloaderApp.Interfaces;
using ExchangeRatesDownloaderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesDownloaderApp.Data
{
    public class DataReader : IDataReader
    {
        private readonly AppDbContext _appDbContext;

        public DataReader(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<ExchangeRateVM>> GetAllRatesAsync()
        {
            List<ExchangeRateVM> results = new();

            if (await _appDbContext.Database.CanConnectAsync())
            {

                var data = await _appDbContext
                .ExchangeTables
                .Include(x => x.Rates)
                .GroupBy(x => x.Type)
                .Select(x => x.OrderByDescending(x => x.EffectiveDate).FirstOrDefault())
                .ToListAsync();

                var objC = data.Where(t => string.Equals(t.Type, "C", StringComparison.CurrentCultureIgnoreCase)).ToList();


                foreach (var table in data.Where(t => !string.Equals(t.Type, "c", StringComparison.CurrentCultureIgnoreCase)))
                {
                    foreach (var rate in table.Rates)
                    {
                        ExchangeRateVM exchangeRate = new()
                        {
                            Name = rate.Name,
                            Code = rate.Code,
                            Bid = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].Rates.Where(r => r.Code == rate.Code).Select(r => r.Bid).First() : rate.Bid,
                            Ask = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].Rates.Where(r => r.Code == rate.Code).Select(r => r.Ask).First() : rate.Ask,
                            Mid = rate.Mid,
                            No = objC[0].Rates.Any(r => r.Code == rate.Code) ? String.Join("\n", table.No, objC[0].No) : table.No,
                            Type = objC[0].Rates.Any(r => r.Code == rate.Code) ? String.Join("\n", table.Type, objC[0].Type) : table.Type,
                            TradingDate = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].TradingDate : table.TradingDate,
                            EffectiveDate = table.EffectiveDate,
                            EffectiveDateBidAsk = objC[0].Rates.Any(r => r.Code == rate.Code) ? objC[0].EffectiveDate : null
                        };
                        results.Add(exchangeRate);
                    }
                }
            }

            return results;
        }
    }
}