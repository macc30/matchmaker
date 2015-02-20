using System;
using System.Linq;
using System.Collections.Generic;

namespace MatchMaker.Core
{
    public static class Extensions
    {
        public static void Times(this Int32 value, Action block)
        {
            for (int x = 0; x < value; x++)
            {
                block();
            }
        }

        public static Double Variance(this IEnumerable<Double> values)
        {
            if (values == null || !values.Any())
                return default(Double);

            Double average = values.Average();
            Double variance = 0.0d;
            foreach (Double value in values)
            {
                variance = variance +
                    (
                        (value - average) * (value - average)
                    );
            }

            return variance / (Double)values.Count();
        }

        public static Double StdDev(this IEnumerable<Double> values)
        {
            if (values == null || !values.Any())
                return default(Double);

            return System.Math.Sqrt(Variance(values));
        }
    }
}