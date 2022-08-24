using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ExchangeRatesDownloaderApp.Data
{
    public class ExchangeRatesTable
    {
        [Display(Name = "Numer tabeli")]
        [JsonProperty("no")]
        public string No { get; set; }

        [Display(Name = "Typ tabeli")]
        [JsonProperty("table")]
        public string Type { get; set; }

        [Display(Name = "data notowania")]
        [JsonProperty("tradingDate")]
        public DateTime? TradingDate { get; set; }

        [Display(Name = "data publikacji")]
        [JsonProperty("effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [Display(Name = " lista kursów poszczególnych walut w tabeli")]
        [JsonProperty("rates")]
        public List<Rate> Rates { get; set; }
    }
}
