using System;
using System.Collections.Generic;

namespace COVIDData.Extensions
{
    public static class DictionaryExcentions
    {
        public static void AddOrUpdateValue(this Dictionary<DateTime, int> dict, DateTime key, int value)
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
    }
}
