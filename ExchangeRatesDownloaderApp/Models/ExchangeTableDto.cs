using Newtonsoft.Json;

namespace ExchangeRatesDownloaderApp.Models
{
    public class ExchangeTableDto
    {
        [JsonProperty("no")]
        public string No { get; set; } = string.Empty;

        [JsonProperty("table")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("tradingDate")]
        public DateTime? TradingDate { get; set; }

        [JsonProperty("effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [JsonProperty("rates")]
        public List<ExchangeRateDto> Rates { get; set; } = new List<ExchangeRateDto>();
    }
}
