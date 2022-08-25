using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ExchangeRatesDownloaderApp.Data
{
    public class ExchangeRatesTable
    {
        [JsonProperty("no")]
        public string No { get; set; } = String.Empty;

        [JsonProperty("table")]
        public string Type { get; set; } = String.Empty;

        [JsonProperty("tradingDate")]
        public DateTime? TradingDate { get; set; }

        [JsonProperty("effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [JsonProperty("rates")]
        public List<Rate> Rates { get; set; } = new List<Rate>();
    }
}
