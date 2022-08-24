using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesDownloaderApp.Data
{
    public class ExchangeRate
    {
        [Display(Name = "Identyfikator")]
        public Guid Id { get; set; }

        [Display(Name = "Numer tabeli")]
        public string No { get; set; }

        [Display(Name = "Typ tabeli")]
        public string Type { get; set; }

        [Display(Name = "data notowania")]
        public DateTime? TradingDate { get; set; }

        [Display(Name = "data publikacji")]
        public DateTime EffectiveDate { get; set; }

        [Display(Name = "nazwa waluty")]
        public string Name { get; set; }

        [Display(Name = "kod waluty")]
        public string Code { get; set; }

        [Display(Name = "przeliczony kurs kupna waluty")]
        public decimal? Bid { get; set; }

        [Display(Name = "przeliczony kurs sprzedaży waluty")]
        public decimal? Ask { get; set; }

        [Display(Name = "przeliczony kurs średni waluty")]
        public decimal Mid { get; set; }
    }
}