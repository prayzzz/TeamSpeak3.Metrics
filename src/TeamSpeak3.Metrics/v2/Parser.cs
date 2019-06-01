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
        private static readonly Regex KeyValueRegex = new Regex("(?<key>\\S+)=(?<value>\\S+)");
        private static readonly Dictionary<Type, List<PropertyInfo>> Setters = new Dictionary<Type, List<PropertyInfo>>();

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

        private static bool IsMatch(this MemberInfo parameter, string name)
        {
            var name1 = name;
            var name2 = name.Replace("_", "").Replace("-", "");

            return string.Equals(parameter.Name, name1, StringComparison.InvariantCultureIgnoreCase)
                   || string.Equals(parameter.Name, name2, StringComparison.InvariantCultureIgnoreCase);
        }

        private static T Parse<T>(string item) where T : new()
        {
            var obj = new T();
            var parameters = new object[] { null };

            var setters = Setters.ComputeIfAbsent(typeof(T), t => t.GetProperties().ToList());
            var matches = KeyValueRegex.Matches(item);

            foreach (Match m in matches)
            {
                var name = m.Groups["key"].Value.Trim();
                var value = m.Groups["value"].Value.Trim();

                foreach (var setter in setters)
                {
                    if (setter.IsMatch(name))
                    {
                        Set(parameters, value, setter, obj);
                        break;
                    }
                }
            }

            return obj;
        }

        private static void Set<T>(object[] parameters, string value, PropertyInfo setter, T obj) where T : new()
        {
            var intType = typeof(int);
            var stringType = typeof(string);
            var doubleType = typeof(double);

            if (setter.PropertyType == intType)
            {
                parameters[0] = int.Parse(value);
                setter.GetSetMethod().Invoke(obj, parameters);
            }
            else if (setter.PropertyType == stringType)
            {
                parameters[0] = Replacer.Replace(value);
                setter.GetSetMethod().Invoke(obj, parameters);
            }
            else if (setter.PropertyType == doubleType)
            {
                parameters[0] = double.Parse(value);
                setter.GetSetMethod().Invoke(obj, parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException(setter.PropertyType.ToString());
            }
        }
    }
}
