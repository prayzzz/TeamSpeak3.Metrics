using System;
using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Common
{
    internal static class Extensions
    {
        internal static TV ComputeIfAbsent<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, Func<TK, TV> compute)
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
