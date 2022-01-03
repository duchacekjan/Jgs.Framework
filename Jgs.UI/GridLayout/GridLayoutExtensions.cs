using System;
using System.Collections.Generic;
using System.Windows;

namespace Jgs.UI.GridLayout
{
    public static class GridLayoutExtensions
    {

        internal static string[] GetParts(this string value, string separator)
        {
#if NET48
            return value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
#else
            return value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
#endif
        }

        internal static void SetGridLayoutValue(this FrameworkElement element, DependencyProperty property, LayoutArgumentType type)
        {
            var arguments = element.GetValue(property) as string;
            var value = arguments.GetSizeFromArguments(type);
            if (value.HasValue)
            {
                var targetProperty = GridLayout.ToGridPropertiesMap[property];
                element.SetValue(targetProperty, value);
            }
        }

        internal static int? GetSizeFromArguments(this string arguments, LayoutArgumentType type)
        {
            var map = arguments.ToLayoutTypeMap();
            int? result = null;
            if (map.ContainsKey(type))
            {
                result = map[type];
            }
            return result;
        }

        internal static IDictionary<LayoutArgumentType, int?> ToLayoutTypeMap(this string arguments)
        {
            var result = new Dictionary<LayoutArgumentType, int?>();
            if (!string.IsNullOrEmpty(arguments))
            {
                var parts = arguments.GetParts(".");
                foreach (var part in parts)
                {
                    (var type, var value) = ParseArgument(part);
                    result.Add(type, value);
                }
            }

            return result;
        }

        private static (LayoutArgumentType, int?) ParseArgument(string argument)
        {
            var parts = argument.GetParts("-");

            if (parts.Length > 0 && parts.Length <= 2)
            {
                var type = parts[0].ToLayoutArgumentType();

                var value = parts.Length > 1
                    ? int.Parse(parts[1])
                    : (int?)null;
                return (type, value);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(argument), "Invalid argument format.");
            }
        }

        internal static void Merge(this IDictionary<LayoutArgumentType, int?> dictionary, IDictionary<LayoutArgumentType, int?> newData)
        {
            foreach (var item in newData)
            {
                dictionary.AddOrUpdate(item);
            }
        }

        private static void AddOrUpdate(this IDictionary<LayoutArgumentType, int?> dictionary, KeyValuePair<LayoutArgumentType, int?> pair)
        {
            if (dictionary.ContainsKey(pair.Key))
            {
                dictionary[pair.Key] = pair.Value;
            }
            else
            {
                dictionary.Add(pair.Key, pair.Value);
            }
        }

        internal static void AddOrUpdate(this IDictionary<LayoutArgumentType, int?> dictionary, LayoutArgumentType type, int? value)
        {
            if (dictionary.ContainsKey(type))
            {
                dictionary[type] = value;
            }
            else
            {
                dictionary.Add(type, value);
            }
        }
    }
}
