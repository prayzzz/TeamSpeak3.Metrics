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

        private static readonly Dictionary<Type, IEnumerable<ConstructorInfo>> TypeInfos = new Dictionary<Type, IEnumerable<ConstructorInfo>>();

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

        public static IEnumerable<T> ToData<T>(string response)
        {
            var items = response.Trim().Split("|");

            var typeInfo = TypeInfos.ComputeIfAbsent(typeof(T), t => t.GetConstructors().ToList());

            return items.Select(x => Parse<T>(typeInfo, x));
        }

        private static T Parse<T>(IEnumerable<ConstructorInfo> constructors, string item)
        {
            var strings = item.Trim().Split(" ");
            var dict = strings.ToDictionary(x => x.Split("=")[0].ToLower(), x => Replacer.Replace(x.Split("=")[1]));

            foreach (var constructorInfo in constructors)
            {
                var p = GetParameters(constructorInfo, dict);
                if (p != null)
                {
                    return (T) constructorInfo.Invoke(p.ToArray());
                }
            }

            throw new Exception($"Couldn't create {typeof(T).Name}");
        }

        private static List<object> GetParameters(ConstructorInfo constructorInfo, Dictionary<string, string> dict)
        {
            var values = new List<object>();

            foreach (var (name, value) in dict)
            {
                foreach (var parameter in constructorInfo.GetParameters())
                {
                    if (parameter.IsMatch(name))
                    {
                        values.Add(value);
                    }
                }
            }

            if (values.Count == constructorInfo.GetParameters().Length)
            {
                return values;
            }

            return null;
        }
    }

    public static class Exteions
    {
        public static bool IsMatch(this ParameterInfo parameter, string name)
        {
            var name1 = name;
            var name2 = name.Replace("_", "").Replace("-", "");

            return string.Equals(parameter.Name, name1, StringComparison.InvariantCultureIgnoreCase)
                   || string.Equals(parameter.Name, name2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
