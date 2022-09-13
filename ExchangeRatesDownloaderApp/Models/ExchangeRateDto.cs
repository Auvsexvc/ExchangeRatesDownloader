using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeRatesDownloaderApp.Models
{
    public class ExchangeRateDto
    {

        [JsonProperty("currency")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("bid")]
        public decimal? Bid { get; set; }

        [JsonProperty("ask")]
        public decimal? Ask { get; set; }

        [JsonProperty("mid")]
        public decimal Mid { get; set; }
    }
}
