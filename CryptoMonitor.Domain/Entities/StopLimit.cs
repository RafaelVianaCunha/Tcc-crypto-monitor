using System;

namespace CryptoMonitor.Domain.Entities
{
    public class StopLimit
    {
        public Guid Id { get; set; }

        public decimal Stop { get; set; }

        public DateTime DeletedAt { get; set; }
    }
}