using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeRatesDownloaderApp.Entities
{
    public class ExchangeRate
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public decimal? Bid { get; set; }

        public decimal? Ask { get; set; }

        public decimal? Mid { get; set; }

        public int TableId { get; set; }

        [ForeignKey("TableId")]
        public ExchangeTable? ExchangeTable { get; set; }
    }
}