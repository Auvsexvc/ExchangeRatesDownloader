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
    }
}