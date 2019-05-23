using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TeamSpeak3.Metrics.v2;

namespace TeamSpeak3.Metrics.Query
{
    internal class QueryResponse<T> : QueryResponse where T : new()
    {
        public QueryResponse(string response) : base(response)
        {
            var data = response.Split('|').Select(ExtractData);
            Data = DataMapper.Map<T>(data);
        }

        public T Data { get; }
    }

    internal class QueryResponse
    {
        public const string NewLine = "\n\r";

        private static readonly Regex KeyValuePattern = new Regex(@"(?<key>\w+)=(?<value>.+)");
        private static readonly string[] Separator = { NewLine };

        public QueryResponse(string response)
        {
            var lines = response.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

            MapErrorLine(Array.Find(lines, l => l.StartsWith("error")));
        }

        public int ErrorId { get; private set; } = -1;

        public string ErrorMessage { get; private set; } = string.Empty;

        public bool IsError => ErrorId != 0;

        public bool IsSuccess => ErrorId == 0;

        protected static Dictionary<string, string> ExtractData(string objectLine)
        {
            var data = new Dictionary<string, string>();

            foreach (var val in objectLine.Split(' '))
            {
                var match = KeyValuePattern.Match(val);

                if (!match.Success)
                {
                    continue;
                }

                data[match.Groups["key"].Value] = match.Groups["value"].Value;
            }

            return data;
        }

        private void MapErrorLine(string errorLine)
        {
            if (string.IsNullOrEmpty(errorLine))
            {
                return;
            }

            var data = ExtractData(errorLine);

            if (data.TryGetValue("id", out var errorIdString) && int.TryParse(errorIdString, out var errorId))
            {
                ErrorId = errorId;
            }

            if (data.TryGetValue("msg", out var msg))
            {
                ErrorMessage = Replacer.Replace(msg);
            }
        }
    }
}