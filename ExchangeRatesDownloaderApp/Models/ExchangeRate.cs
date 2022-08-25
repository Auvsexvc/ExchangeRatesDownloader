using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesDownloaderApp.Models
{
    public class ExchangeRate
    {
        [Display(Name = "Numer tabeli")]
        public string No { get; set; } = string.Empty;

        [Display(Name = "Typ tabeli")]
        public string Type { get; set; } = string.Empty;

        [Display(Name = "data notowania")]
        public DateTime? TradingDate { get; set; }

        [Display(Name = "data publikacji")]
        public DateTime EffectiveDate { get; set; }

        [Display(Name = "nazwa waluty")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "kod waluty")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "przeliczony kurs kupna waluty")]
        public decimal? Bid { get; set; }

        [Display(Name = "przeliczony kurs sprzedaży waluty")]
        public decimal? Ask { get; set; }

        [Display(Name = "przeliczony kurs średni waluty")]
        public decimal Mid { get; set; }
    }
}