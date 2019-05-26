using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TeamSpeak3.Metrics.v2
{
    public static class Parser
    {
        private static readonly Regex BooleanResponseRegex = new Regex("id=(?<id>[0-9]+) msg=(?<msg>.+)");
        private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> Setters = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        public static BooleanResponse ToBooleanResponse(string response)
        {
            var match = BooleanResponseRegex.Match(response);
            if (!match.Groups["id"].Success)
            {
                throw new Exception($"Id not found in response '{response}'");
            }

            if (!match.Groups["msg"].Success)
            {
                throw new Exception($"Message not found in response '{response}'");
            }

            return new BooleanResponse(int.Parse(match.Groups["id"].Value), match.Groups["msg"].Value);
        }

        public static IEnumerable<T> ToData<T>(string response) where T : new()
        {
            var items = response.Trim().Split("|");
            return items.Select(x => Parse<T>(x));
        }

        private static T Parse<T>(string item) where T : new()
        {
            var obj = new T();
            var parameters = new object[] { null };

            var setters = Setters.ComputeIfAbsent(typeof(T), t => t.GetProperties().ToList());

            var strings = item.Trim().Split(" ");
            var dict = strings.ToDictionary(x => x.Split("=")[0], x => Replacer.Replace(x.Split("=")[1]));

            foreach (var (name, value) in dict)
            {
                foreach (var setter in setters)
                {
                    if (setter.IsMatch(name))
                    {
                        parameters[0] = value;
                        setter.GetSetMethod().Invoke(obj, parameters);
                        break;
                    }
                }
            }

            return obj;
        }

        private static bool IsMatch(this MemberInfo parameter, string name)
        {
            var name1 = name;
            var name2 = name.Replace("_", "").Replace("-", "");

            return string.Equals(parameter.Name, name1, StringComparison.InvariantCultureIgnoreCase)
                   || string.Equals(parameter.Name, name2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}