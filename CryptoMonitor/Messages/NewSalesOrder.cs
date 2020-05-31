using System;
using CryptoMonitor.Entities;

namespace CryptoMonitor.Messages 
{
    public class NewSalesOrder
    {
        public Guid UserId { get; set; }

        public Coin Coin { get; set; }

        public decimal Stop { get; set; }

        public decimal Limit { get; set; }

        public decimal Quantity { get; set; }
    }
}