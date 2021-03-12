using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CryptoMonitor.Domain.Services.Interfaces;
using CryptoMonitor.Enums;

namespace CryptoMonitor.Domain.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        public IDictionary<Exchanges, decimal> values { get; }

        public PriceCalculator()
        {
            values = new ConcurrentDictionary<Exchanges, decimal>();
        }

        public decimal CalculateMedianForNewCoinValue(decimal amount, Exchanges exchange)
        {
           if (values.TryGetValue(exchange, out _))
            {
                values[exchange] = amount;
            }
            else
            {
                values.Add(exchange, amount);
            }

            var median = GetMedian(values.Select(v => v.Value).ToList());

            return median;
        }

        public decimal GetMedian(IList<decimal> numbers)
        {
            if(!numbers.ToArray<decimal>().Length.Equals(Enum.GetNames(typeof(Exchanges)).Length)){
                return numbers.Max();
            }

            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;

            var sortedNumbers = numbers.OrderBy(n => n);

            decimal median;

            if ((numberCount % 2) == 0)
            {
                var num1 = sortedNumbers.ElementAt(halfIndex);
                var num2 = sortedNumbers.ElementAt(halfIndex - 1);

                median = (num1 + num2) / 2;
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }
    }
}