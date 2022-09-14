using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesDownloaderApp.Models
{
    public class ExchangeRateVM
    {
        private decimal? bid;
        private decimal? ask;
        private decimal? mid;

        [Display(Name = "Source table number(s)")]
        public string No { get; set; } = string.Empty;

        [Display(Name = "Source table type(s)")]
        public string Type { get; set; } = string.Empty;

        [Display(Name = "Trading date for buy/sell exchange rate)")]
        public DateTime? TradingDate { get; set; }

        [Display(Name = "Publication date for average exchange rate")]
        public DateTime EffectiveDate { get; set; }

        [Display(Name = "Publication date for buy/sell exchange rate")]
        public DateTime? EffectiveDateBidAsk { get; set; }

        [Display(Name = "Currency name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Currency code")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "Buy exchange rate")]
        public decimal? Bid
        {
            get { return bid != null ? TrimDecimalZeroes((decimal)bid) : bid; }
            set { bid = value; }
        }

        [Display(Name = "Sell exchange rate")]
        public decimal? Ask
        {
            get { return ask != null ? TrimDecimalZeroes((decimal)ask) : ask; }
            set { ask = value; }
        }

        [Display(Name = "Average exchange rate")]
        public decimal? Mid
        {
            get { return mid != null ? TrimDecimalZeroes((decimal)mid) : mid; }
            set { mid = value; }
        }

        private static decimal TrimDecimalZeroes(decimal dec)
        {
            string number = Convert.ToString(dec);
            if (number.Contains('.'))
            {
                while (number[^1..] == "0" && number.Split('.')[1].Length > 4)
                {
                    number = number[..^1];
                }
            }
            return Convert.ToDecimal(number);
        }
    }
}