using System;
using System.Collections.Generic;

namespace Jgs.UI.GridLayout
{
    public enum LayoutArgumentType
    {
        Compact,
        Standard,
        Wide
    }

    public static class LayoutArgumentTypeExtensions
    {
#if NET6_0
        private static Dictionary<string, LayoutArgumentType> m_mapTo = new()
#else
        private static Dictionary<string, LayoutArgumentType> m_mapTo = new Dictionary<string, LayoutArgumentType>()
#endif

        {
            { "c", LayoutArgumentType.Compact },
            { "s", LayoutArgumentType.Standard },
            { "w", LayoutArgumentType.Wide },
        };

        public static LayoutArgumentType ToLayoutArgumentType(this string value)
        {
            if (m_mapTo.TryGetValue(value, out var type))
            {
                return type;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Invalid enum value '{value}'.");
            }
        }
    }
}
