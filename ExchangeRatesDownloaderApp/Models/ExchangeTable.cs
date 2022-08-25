using Newtonsoft.Json;

namespace ExchangeRatesDownloaderApp.Models
{
    public class ExchangeTable
    {
        public int Id { get; set; }

        [JsonProperty("no")]
        public string No { get; set; } = string.Empty;

        [JsonProperty("table")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("tradingDate")]
        public DateTime? TradingDate { get; set; }

        [JsonProperty("effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [JsonProperty("rates")]
        public List<ExchangeRate> Rates { get; set; } = new List<ExchangeRate>();
    }
}