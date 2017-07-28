using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TeamSpeak3.Metrics.Query
{
    public class DataMapper
    {
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> TypeToSetters = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        private static readonly Type TypeInt = typeof(int);
        private static readonly Type TypeDouble = typeof(double);
        private static readonly Type TypeBool = typeof(bool);

        public static T Map<T>(Dictionary<string, string> data) where T : new()
        {
            var dataType = typeof(T);

            if (IsList(dataType))
            {
                throw new NotSupportedException();
            }

            CacheType(dataType);

            var instance = (T)Activator.CreateInstance(dataType);

            var setters = TypeToSetters[dataType];
            foreach (var dataPair in data)
            {
                var name = GetPropertyName<T>(dataPair.Key);
                if (setters.TryGetValue(name, out var property))
                {
                    SetValue(instance, property, Replacer.Replace(dataPair.Value.Trim()));
                }
            }

            return instance;
        }

        private static void SetValue<T>(T instance, PropertyInfo property, string value)
        {
            object typedValue = value;
            if (TypeInt == property.PropertyType)
            {
                typedValue = int.TryParse(value, out var parsedValue) ? parsedValue : throw new ArgumentException($"Couldn't parse {value} to int");
            }

            if (TypeDouble == property.PropertyType)
            {
                typedValue = double.TryParse(value, out var parsedValue) ? parsedValue : throw new ArgumentException($"Couldn't parse {value} to double");
            }

            if (TypeBool == property.PropertyType)
            {
                typedValue = bool.TryParse(value, out var parsedValue) ? parsedValue : throw new ArgumentException($"Couldn't parse {value} to bool");
            }
            
            property.SetValue(instance, typedValue);
        }

        private static string GetPropertyName<T>(string name) where T : new()
        {
            return name.Replace("_", "");
        }

        private static void CacheType(Type type)
        {
            if (TypeToSetters.ContainsKey(type))
            {
                return;
            }

            var setters = type.GetProperties()
                              .Where(p => p.CanWrite)
                              .ToDictionary(property => property.Name.Trim().ToLower());

            TypeToSetters.Add(type, setters);
        }

        private static bool IsList(Type givenType)
        {
            return typeof(IEnumerable).IsAssignableFrom(givenType);
        }
    }
}