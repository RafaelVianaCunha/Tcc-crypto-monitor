using System;
using System.Collections.Generic;
using CryptoMonitor.Domain.Interfaces;

namespace CryptoMonitor.Domain
{
    public static class RunningMonitors
    {
        public static IDictionary<Guid, IList<IExchangeClient>> Monitors = new Dictionary<Guid, IList<IExchangeClient>>();
    }
}