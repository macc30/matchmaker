using System;
using System.Linq;
using System.Collections.Generic;

namespace MatchMaker.Core
{
    public static class Extensions
    {
        public static double MS_PER_SECOND = 1000.0;
        public static double MS_PER_MINUTE = 60.0 * 1000.0;
        public static double MS_PER_HOUR = 60.0 * 60.0 * 1000.0;

        public static String ClockFormat(this long clock_value)
        {
            if (clock_value < MS_PER_SECOND)
            {
                return String.Format("{0} ms", clock_value);
            }
            else if (clock_value < MS_PER_MINUTE)
            {
                return String.Format("{0}.{1} sec", Math.Floor((double)clock_value / 1000.0), clock_value % 1000);
            }
            else if (clock_value < MS_PER_HOUR)
            {
                var minutes = Math.Floor((clock_value) / MS_PER_MINUTE);
                var minutes_in_ms = minutes * MS_PER_MINUTE;
                var seconds = Math.Floor((clock_value - minutes_in_ms) / MS_PER_SECOND);
                var seconds_in_ms = seconds * MS_PER_SECOND;
                var ms = clock_value - minutes_in_ms - seconds_in_ms;
                return String.Format("{0} min {1}.{2} sec", minutes, seconds, ms);
            }
            else
            {
                var hours = Math.Floor((double)clock_value / MS_PER_HOUR);
                var hours_in_ms = hours * MS_PER_HOUR;
                var minutes = Math.Floor((clock_value - hours_in_ms) / MS_PER_MINUTE);
                var minutes_in_ms = minutes * MS_PER_MINUTE;
                var seconds = Math.Floor((clock_value - hours_in_ms - minutes_in_ms) / MS_PER_SECOND);
                var seconds_in_ms = seconds * MS_PER_SECOND;
                var ms = clock_value - hours_in_ms - minutes_in_ms - seconds_in_ms;

                return String.Format("{0} hr {1} min {2}.{3} sec", hours, minutes, seconds, ms);
            }
        }

        public static T PopFront<T>(this List<T> values)
        {
            if (values.Any())
            {
                var value = values[0];
                values.RemoveAt(0);
                return value;
            }
            return default(T);
        }

        public static T PeekFront<T>(this List<T> values)
        {
            if (values.Any())
            {
                var value = values[0];
                return value;
            }
            return default(T);
        }

        public static T PopBack<T>(this List<T> values)
        {
            if (values.Any())
            {
                var value = values.Last();
                values.RemoveAt(values.Count - 1);
                return value;
            }
            return default(T);
        }

        public static void PushFront<T>(this List<T> values, T value)
        {
            values.Insert(0, value);
        }

        public static void PushBack<T>(this List<T> values, T value)
        {
            values.Add(value);
        }

        public static T WeightedRandomSelection<T>(this List<T> values, Func<T, Double> weightSelector, Random randomProvider = null)
        {
            var total = values.Sum(_ => weightSelector(_));

            var cumulative_weights = new List<Double>(capacity: values.Count);
            var running_total = 0.0;
            for(var x = 0; x < values.Count; x++)
            {
                var normalized_weight = weightSelector(values[x]) / total;
                running_total += normalized_weight;
                cumulative_weights.Add(running_total);
            }

            randomProvider = randomProvider ?? new Random(); //this is fine for one off calls, but for repeated calls close to eachother use a random provider.
            var normalized_value = randomProvider.NextDouble();
            for (var x = 0; x < cumulative_weights.Count; x++) //a bst here would be better.
            {
                if (normalized_value < cumulative_weights[x])
                {
                    return values[x];
                }
            }

            return values.Last();
        }

        public static void Times(this Int32 value, Action block)
        {
            for (int x = 0; x < value; x++)
            {
                block();
            }
        }

        public static Double Variance(this IEnumerable<long> values)
        {
            if (values == null || !values.Any())
                return default(long);

            Double average = values.Average();
            Double variance = 0.0d;
            foreach (long value in values)
            {
                variance = variance +
                    (
                        (value - average) * (value - average)
                    );
            }

            return variance / (Double)values.Count();
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

        public static Double StdDev(this IEnumerable<long> values)
        {
            if (values == null || !values.Any())
                return default(long);

            return System.Math.Sqrt(Variance(values));
        }

        public static Double StdDev(this IEnumerable<Double> values)
        {
            if (values == null || !values.Any())
                return default(Double);

            return System.Math.Sqrt(Variance(values));
        }
    }
}