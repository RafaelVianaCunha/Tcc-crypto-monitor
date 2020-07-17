using CryptoMonitor.Enums;

namespace CryptoMonitor.Domain.ValueObjects
{
    public class OrderBook
    {
        public string Symbol { get; set; }

        public string Main { get; set; }

        public decimal Amount { get; set; }

        public Exchanges Exchange { get; set; }
    }
}