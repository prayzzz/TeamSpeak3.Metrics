using System;
using System.Collections.Generic;

namespace TeamSpeak3.Metrics.v2
{
    public static class DictionaryExtensions
    {
        public static TV ComputeIfAbsent<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, Func<TK, TV> compute)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            var newValue = compute(key);
            if (newValue == null)
            {
                return default;
            }

            dictionary[key] = newValue;
            return newValue;
        }
    }
}
