using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Query
{
    public class Replacer
    {
        private static readonly IReadOnlyDictionary<string, string> EscapeChars = new Dictionary<string, string>
        {
            { @"\/", "/" },
            { @"\s", " " },
            { @"\p", "|" }
        };

        public static string Replace(string value)
        {
            foreach (var escapePair in EscapeChars)
            {
                value = value.Replace(escapePair.Key, escapePair.Value);
            }

            return value;
        }
    }
}