using System;
using CryptoMonitor.Domain.Entities;

namespace CryptoMonitor.Domain.ValueObjects
{
    public class OrderSale
    {
        public Guid StopLimitId { get; set; }

        public StopLimit StopLimit { get; set; }
    }
}