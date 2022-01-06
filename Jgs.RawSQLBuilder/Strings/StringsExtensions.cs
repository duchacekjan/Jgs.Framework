using System;

namespace Jgs.RawSQLBuilder.Strings
{
    internal static class StringExtensions
    {
        internal static string[] GetParts(this string value, string separator)
        {
            var result = Array.Empty<string>();
            if (!string.IsNullOrEmpty(value))
            {
#if NET48
                result = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
#else
                result = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
#endif                
            }

            return result;
        }

        internal static bool HasNonASCIIChars(this string text)
        {
            return (System.Text.Encoding.UTF8.GetByteCount(text) != text.Length);
        }
    }
}
