using System;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Entities
{
    public class Coin
    {
        public string Name { get; set; }
        public string Main  { get; set; }
        public decimal Amount { get; set; }

        public Exchange Exchange { get; set; }
    }
}