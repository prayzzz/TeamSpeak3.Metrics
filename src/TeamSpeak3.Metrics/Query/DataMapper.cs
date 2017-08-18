using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TeamSpeak3.Metrics.Query
{
    public static class DataMapper
    {
        public static T Map<T>(IEnumerable<Dictionary<string, string>> data)
        {
            var preparedData = PrepareData<T>(data);

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

        private static List<Dictionary<string, string>> PrepareData<T>(IEnumerable<Dictionary<string, string>> data)
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

        //private static readonly Type TypeBool = typeof(bool);

        //private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> TypeToSetters = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        //private static readonly Type TypeInt = typeof(int);

        //private static readonly Type TypeDouble = typeof(double);

        //public static T Map<T>(IEnumerable<Dictionary<string, string>> data) where T : new()
        //{
        //    var targetType = typeof(T);
        //    var isList = IsList(targetType);

        //    var dataType = targetType;
        //    if (isList)
        //    {
        //        if (dataType.GenericTypeArguments.Length != 1)
        //        {
        //            throw new NotSupportedException("IEnumerable with one TypeArgument required");

        //        }

        //        dataType = dataType.GenericTypeArguments.FirstOrDefault();
        //    }

        //    CacheType(dataType);

        //    var mappedObjects = new List<object>();
        //    foreach (var objectData in data)
        //    {
        //        var instance = (T)Activator.CreateInstance(targetType);

        //        var setters = TypeToSetters[targetType];
        //        foreach (var dataPair in objectData)
        //        {
        //            var name = GetPropertyName<T>(dataPair.Key);
        //            if (setters.TryGetValue(name, out var property))
        //            {
        //                SetValue(instance, property, Replacer.Replace(dataPair.Value.Trim()));
        //            }
        //        }
        //    }

        //    return instance;
        //}

        //private static void SetValue<T>(T instance, PropertyInfo property, string value)
        //{
        //    object typedValue = value;
        //    if (TypeInt == property.PropertyType)
        //    {
        //        typedValue = int.TryParse(value, out var parsedValue) ? parsedValue : throw new ArgumentException($"Couldn't parse {value} to int");
        //    }

        //    if (TypeDouble == property.PropertyType)
        //    {
        //        typedValue = double.TryParse(value, out var parsedValue) ? parsedValue : throw new ArgumentException($"Couldn't parse {value} to double");
        //    }

        //    if (TypeBool == property.PropertyType)
        //    {
        //        typedValue = bool.TryParse(value, out var parsedValue) ? parsedValue : throw new ArgumentException($"Couldn't parse {value} to bool");
        //    }

        //    property.SetValue(instance, typedValue);
        //}

        //private static void CacheType(Type type)
        //{
        //    if (TypeToSetters.ContainsKey(type))
        //    {
        //        return;
        //    }

        //    var setters = type.GetProperties()
        //                      .Where(p => p.CanWrite)
        //                      .ToDictionary(property => property.Name.Trim().ToLower());

        //    TypeToSetters.Add(type, setters);
        //}
    }
}