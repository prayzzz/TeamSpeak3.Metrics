using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TeamSpeak3.Metrics.Connection
{
    public class QueryResponse<T> : QueryResponse
    {
        public QueryResponse(string response) : base(response)
        {
        }

        public T Data { get; set; }
    }

    public class QueryResponse
    {
        private static readonly string[] Separator = { Environment.NewLine };

        private static readonly Regex KeyValuePattern = new Regex($@"(?<key>\w+)=(?<value>.+)");

        public QueryResponse(string response)
        {
            var lines = response.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

            EvaluateErrorLine(lines.FirstOrDefault(l => l.StartsWith("error")));
        }

        public int ErrorId { get; set; } = -1;

        public string ErrorMessage { get; set; } = string.Empty;

        public bool HasError => ErrorId != 0;

        private void EvaluateErrorLine(string errorLine)
        {
            if (string.IsNullOrEmpty(errorLine))
            {
                return;
            }

            var pairs = errorLine.Split(' ');

            var data = new Dictionary<string, string>();
            foreach (var val in pairs)
            {
                var match = KeyValuePattern.Match(val);

                if (!match.Success)
                {
                    continue;
                }

                data[match.Groups["key"].Value] = match.Groups["value"].Value;
            }

            if (data.TryGetValue("id", out var errorIdString))
            {
                if (int.TryParse(errorIdString, out var errorId))
                {
                    ErrorId = errorId;
                }
            }

            if (data.TryGetValue("msg", out var msg))
            {
                ErrorMessage = msg;
            }
        }
    }
}