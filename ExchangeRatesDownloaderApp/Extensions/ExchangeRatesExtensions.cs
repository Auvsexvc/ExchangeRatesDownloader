using ExchangeRatesDownloaderApp.Entities;
using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Extensions
{
    public static class ExchangeRatesExtensions
    {
        public static ExchangeTableDto ToDto(this ExchangeTable exchangeTable)
        {
            return new ExchangeTableDto()
            {
                No = exchangeTable.No,
                Type = exchangeTable.Type,
                TradingDate = exchangeTable.TradingDate,
                EffectiveDate = exchangeTable.EffectiveDate,
                Rates = exchangeTable.Rates.ConvertAll(r => new ExchangeRateDto()
                {
                    Name = r.Name,
                    Code = r.Code,
                    Bid = r.Bid,
                    Ask = r.Ask,
                    Mid = r.Mid
                })
            };
        }

        public static ExchangeTable FromDto(this ExchangeTableDto exchangeTableDto)
        {
            return new ExchangeTable()
            {
                No = exchangeTableDto.No,
                Type = exchangeTableDto.Type,
                TradingDate = exchangeTableDto.TradingDate,
                EffectiveDate = exchangeTableDto.EffectiveDate,
                Rates = exchangeTableDto.Rates.ConvertAll(r => new ExchangeRate()
                {
                    Name = r.Name,
                    Code = r.Code,
                    Bid = r.Bid,
                    Ask = r.Ask,
                    Mid = r.Mid
                })
            };
        }

        public static ExchangeRateVM ExtractExchangeRateVMFromTables(this ExchangeRateDto exchangeRateDto, IEnumerable<ExchangeTableDto> exchangeTableDtos)
        {
            var x = exchangeTableDtos
                .GroupBy(k => (Rates: k.Rates.Where(x => x.Code == exchangeRateDto.Code), Table: k))
                .Where(g => g.Key.Rates.Any())
                .Select(g => (Rate: g.Key.Rates.First(), g.Key.Table));

            return new ExchangeRateVM()
            {
                No = String.Join("\n", x.Select(x => x.Table.No)),
                Type = String.Join("\n", x.Select(x => x.Table.Type)),
                TradingDate = x.Select(x => x.Table.TradingDate).FirstOrDefault(x => x != null),
                EffectiveDate = x.Max(x => x.Table.EffectiveDate),
                EffectiveDateBidAsk = x.Select(x => x.Table.TradingDate).FirstOrDefault(x => x != null),
                Name = x.First().Rate.Name,
                Code = x.First().Rate.Code,
                Bid = x.Select(x => x.Rate.Bid).FirstOrDefault(x => x != null),
                Ask = x.Select(x => x.Rate.Ask).FirstOrDefault(x => x != null),
                Mid = x.Select(x => x.Rate.Mid).FirstOrDefault(x => x != null)
            };
        }

        public static IEnumerable<ExchangeRateVM> ConvertToVMs(this IEnumerable<ExchangeTableDto> exchangeTables)
        {
            var exchangeRateVMs = new List<ExchangeRateVM>();
            foreach (var exchangeRateDto in exchangeTables.SelectMany(x => x.Rates))
            {
                exchangeRateVMs.Add(exchangeRateDto.ExtractExchangeRateVMFromTables(exchangeTables));
            }

            return exchangeRateVMs.DistinctBy(x => x.Code);
        }
    }
}