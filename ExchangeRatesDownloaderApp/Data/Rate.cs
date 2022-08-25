using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesDownloaderApp.Data
{
    public class Rate
    {
        [JsonProperty("currency")]
        public string Name { get; set; } = String.Empty;

        [JsonProperty("code")]
        public string Code { get; set; } = String.Empty;

        [JsonProperty("bid")]
        public decimal? Bid { get; set; }

        [JsonProperty("ask")]
        public decimal? Ask { get; set; }

        [JsonProperty("mid")]
        public decimal Mid { get; set; }
    }
}