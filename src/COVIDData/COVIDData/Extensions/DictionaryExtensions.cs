using System;
using System.Collections.Generic;
using System.Linq;

namespace COVIDData.Extensions
{
    public static class DictionaryExcentions
    {
        public static void AddOrUpdateValue(this IDictionary<DateTime, int> dict, DateTime key, int value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += value;
            }
            else
            {
                dict[key] = value;
            }
        }

        public static int GetValueOrMin(this IReadOnlyDictionary<DateTime, int> dict, DateTime key, out DateTime minDate)
        {
            minDate = key;
            if (!dict.TryGetValue(minDate, out var minValue))
            {
                minDate = dict.Keys.Min();
                minValue = dict[minDate];
            }

            return minValue;
        }

        public static int GetValueOrMax(this IReadOnlyDictionary<DateTime, int> dict, DateTime key, out DateTime maxDate)
        {
            maxDate = key;
            if (!dict.TryGetValue(maxDate, out var maxValue))
            {
                maxDate = dict.Keys.Max();
                maxValue = dict[maxDate];
            }

            return maxValue;
        }
    }
}
