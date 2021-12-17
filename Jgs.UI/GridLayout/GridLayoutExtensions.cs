using System;
using System.Collections.Generic;

namespace Jgs.UI.GridLayout
{
    public static class GridLayoutExtensions
    {

        internal static string[] GetParts(this string value, string separator)
        {
#if NET48
            return value.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
#else
            return value.Split(".");
#endif
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
