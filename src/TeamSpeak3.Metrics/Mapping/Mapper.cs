using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TeamSpeak3.Metrics.Common;

namespace TeamSpeak3.Metrics.Mapping
{
    internal static class Mapper
    {
        private static readonly Regex StatusResponseRegex = new Regex("^error id=(?<id>\\d+) msg=(?<msg>\\S+)");
        private static readonly Regex KeyValueRegex = new Regex("(?<key>\\S+)=(?<value>\\S+)");
        private static readonly Dictionary<Type, List<PropertyInfo>> Setters = new Dictionary<Type, List<PropertyInfo>>();

        private static readonly Type DoubleType = typeof(double);
        private static readonly Type IntType = typeof(int);
        private static readonly Type LongType = typeof(ulong);
        private static readonly Type StringType = typeof(string);

        private static readonly string[] NewLine = { "\r\n", "\n", "\r" };

        internal static DataResponse<IEnumerable<T>> ToData<T>(string response) where T : new()
        {
            var lines = response.Trim().Split(NewLine, StringSplitOptions.RemoveEmptyEntries);
            var status = ToStatusResponseInternal(lines.Last());
            if (!status.IsSuccess)
            {
                return new DataResponse<IEnumerable<T>>(status);
            }

            if (lines.Length < 2)
            {
                throw new MetricsException("Response doesn't contain data");
            }

            var items = response.Trim().Split('|');
            return new DataResponse<IEnumerable<T>>(status, items.Select(x => Parse<T>(x)));
        }

        internal static StatusResponse ToStatusResponse(string response)
        {
            var lines = response.Trim().Split(NewLine, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 1)
            {
                throw new MetricsException("Response doesn't contain status line");
            }

            return ToStatusResponseInternal(lines.Last());
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
            if (setter.PropertyType == StringType)
            {
                parameters[0] = Escaper.ReverseEscape(value);
                setter.GetSetMethod().Invoke(obj, parameters);
            }
            else if (setter.PropertyType == IntType)
            {
                if (int.TryParse(value, out var result))
                {
                    parameters[0] = result;
                    setter.GetSetMethod().Invoke(obj, parameters);
                }
                else
                {
                    throw new ArgumentException($"Couldn't parse value: {value} to {setter.PropertyType} for setter {setter.Name}");
                }
            }
            else if (setter.PropertyType == LongType)
            {
                if (ulong.TryParse(value, out var result))
                {
                    parameters[0] = result;
                    setter.GetSetMethod().Invoke(obj, parameters);
                }
                else
                {
                    throw new ArgumentException($"Couldn't parse value: {value} to {setter.PropertyType} for setter {setter.Name}");
                }
            }
            else if (setter.PropertyType == DoubleType)
            {
                if (double.TryParse(value, out var result))
                {
                    parameters[0] = result;
                    setter.GetSetMethod().Invoke(obj, parameters);
                }
                else
                {
                    throw new ArgumentException($"Couldn't parse value: {value} to {setter.PropertyType} for setter {setter.Name}");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(setter.PropertyType.ToString(), $"Setter: {setter.Name}, Value: {value}");
            }
        }

        private static StatusResponse ToStatusResponseInternal(string statusLine)
        {
            var match = StatusResponseRegex.Match(statusLine);
            if (!match.Success)
            {
                throw new MetricsException("Response doesn't contain status line");
            }

            return new StatusResponse(int.Parse(match.Groups["id"].Value), Escaper.ReverseEscape(match.Groups["msg"].Value));
        }
    }
}
