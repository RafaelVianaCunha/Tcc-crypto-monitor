using System;

namespace CryptoMonitor.Entities
{
    public class StopLimit
    {
        String exchangeId { get; set; }
        Coin coin { get; set; }
        decimal stop { get; set; }
        decimal lim { get; set; }
    }
}