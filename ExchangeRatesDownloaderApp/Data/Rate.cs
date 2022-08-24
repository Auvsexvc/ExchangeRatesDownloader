using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesDownloaderApp.Data
{
    public class Rate
    {
        [Display(Name = "nazwa waluty")]
        [JsonProperty("currency")]
        public string Name { get; set; }

        [Display(Name = "kod waluty")]
        [JsonProperty("code")]
        public string Code { get; set; }

        [Display(Name = "przeliczony kurs kupna waluty")]
        [JsonProperty("bid")]
        public decimal? Bid { get; set; }

        [Display(Name = "przeliczony kurs sprzedaży waluty")]
        [JsonProperty("ask")]
        public decimal? Ask { get; set; }

        [Display(Name = "przeliczony kurs średni waluty")]
        [JsonProperty("mid")]
        public decimal Mid { get; set; }
    }
}