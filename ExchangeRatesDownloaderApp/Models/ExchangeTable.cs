namespace ExchangeRatesDownloaderApp.Models
{
    public class ExchangeTable
    {
        public int Id { get; set; }

        public string No { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public DateTime? TradingDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public List<ExchangeRate> Rates { get; set; } = new List<ExchangeRate>();
    }
}