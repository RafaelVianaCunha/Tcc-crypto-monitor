using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CryptoMonitor.Enums;

namespace CryptoMonitor
{
    public class PriceCalculator
    {
        public IDictionary<Exchange, decimal> values { get; }

        public PriceCalculator()
        {
            values = new ConcurrentDictionary<Exchange, decimal>();
        }

        public decimal CalculateMedianForNewCoinValue(decimal newValue, Exchange exchange)
        {
           if (values.TryGetValue(exchange, out _))
            {
                values[exchange] = newValue;
            }
            else
            {
                values.Add(exchange, newValue);
            }

            var median = GetMedian(values.Select(v => v.Value).ToList());

            Console.WriteLine("Mediana " + median);

            return median;
        }

        public decimal GetMedian(IList<decimal> numbers)
        {
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