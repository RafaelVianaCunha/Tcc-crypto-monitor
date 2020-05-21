using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CryptoMonitor {
    public class Calculator {
        public IDictionary<string, decimal> values { get; }
        public decimal StopLimit { get; }

        public Calculator(decimal stopLimit) 
        {
            StopLimit = stopLimit;
            values = new ConcurrentDictionary<string, decimal>();
        }

        public void CalculateMedianForNewCoinValue(decimal newValue, string exchange) 
        {
            Console.WriteLine($"Exchange {exchange} Valor: {newValue}");

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

            if (StopLimit >= median) {
                Console.WriteLine("Tem que vender");
            }
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
            } else {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }
    }
}