using System.Collections.Generic;
using System.Text;

namespace TeamSpeak3.Metrics.Test
{
    public static class Translator
    {
        public static string Translate(Dictionary<string, string> data)
        {
            var builder = new StringBuilder();

            foreach (var pair in data)
            {
                builder.Append(pair.Key).Append("=").Append(pair.Value).Append(" ");
            }

            return builder.ToString().Trim();
        }

        public static string Translate(IEnumerable<Dictionary<string, string>> dataList)
        {
            var builder = new StringBuilder();

            foreach (var data in dataList)
            {
                builder.Append(Translate(data)).Append("|");
            }

            return builder.ToString().Trim('|');
        }
    }
}