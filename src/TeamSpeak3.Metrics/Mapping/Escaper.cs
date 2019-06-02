using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Mapping
{
    internal static class Escaper
    {
        private static readonly IEnumerable<(string, string)> EscapeChars = new List<(string, string)>
        {
            (@"\/", "/"),
            (@"\s", " "),
            (@"\p", "|")
        };

        internal static string ReverseEscape(string value)
        {
            foreach (var (oldValue, newValue) in EscapeChars)
            {
                value = value.Replace(oldValue, newValue);
            }

            return value;
        }
    }
}
