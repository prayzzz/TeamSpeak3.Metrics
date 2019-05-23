using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TeamSpeak3.Metrics.v2;

namespace TeamSpeak3.Metrics.Query
{
    public static class DataMapper
    {
        public static T Map<T>(IEnumerable<Dictionary<string, string>> data)
        {
            var preparedData = PrepareData(data);

            var serializedObject = IsList(typeof(T)) ? JsonConvert.SerializeObject(preparedData) : JsonConvert.SerializeObject(preparedData.FirstOrDefault());
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        private static string GetPropertyName(string name)
        {
            return name.Replace("_", "");
        }

        private static bool IsList(Type givenType)
        {
            return typeof(IEnumerable).IsAssignableFrom(givenType);
        }

        private static List<Dictionary<string, string>> PrepareData(IEnumerable<Dictionary<string, string>> data)
        {
            var preparedData = new List<Dictionary<string, string>>();
            foreach (var dictionary in data)
            {
                var preparedDict = new Dictionary<string, string>();

                foreach (var pair in dictionary)
                {
                    preparedDict[GetPropertyName(pair.Key)] = Replacer.Replace(pair.Value);
                }

                preparedData.Add(preparedDict);
            }

            return preparedData;
        }
    }
}