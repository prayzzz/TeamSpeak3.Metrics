using System.Collections.Generic;

namespace TeamSpeak3.Metrics.v2
{
    public static class Replacer
    {
        private static readonly IReadOnlyDictionary<string, string> EscapeChars = new Dictionary<string, string>
        {
            { @"\/", "/" },
            { @"\s", " " },
            { @"\p", "|" }
        };

        public static string Replace(string value)
        {
            foreach (var (oldValue, newValue) in EscapeChars)
            {
                value = value.Replace(oldValue, newValue);
            }

            return value;
        }
    }
}