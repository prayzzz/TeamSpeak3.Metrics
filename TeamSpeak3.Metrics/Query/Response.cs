using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TeamSpeak3.Metrics.Query
{
    public class Response<T> : Response where T : new()
    {
        public Response(string response) : base(response)
        {
            var data = response.Split('|')
                               .Select(s => ExtractData(s));

            Data = DataMapper.Map<T>(data);
        }

        public T Data { get; }
    }

    public class Response
    {
        public const string NewLine = "\n\r";

        private static readonly string[] Separator = { NewLine };

        private static readonly Regex KeyValuePattern = new Regex(@"(?<key>\w+)=(?<value>.+)");

        public Response(string response)
        {
            var lines = response.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

            MapErrorLine(lines.FirstOrDefault(l => l.StartsWith("error")));
        }

        public int ErrorId { get; private set; } = -1;

        public string ErrorMessage { get; private set; } = string.Empty;

        public bool HasError => ErrorId != 0;

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

            if (data.TryGetValue("id", out var errorIdString))
            {
                if (int.TryParse(errorIdString, out var errorId))
                {
                    ErrorId = errorId;
                }
            }

            if (data.TryGetValue("msg", out var msg))
            {
                ErrorMessage = Replacer.Replace(msg);
            }
        }
    }
}