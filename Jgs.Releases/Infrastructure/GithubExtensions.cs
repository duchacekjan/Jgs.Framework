using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JgsReleases.Infrastructure
{
    /// <summary>
    /// Extensions for github client
    /// </summary>
    internal static class GithubExtensions
    {
        /// <summary>
        /// Default json options
        /// </summary>
        private static JsonSerializerOptions JsonOptions => new JsonSerializerOptions
        {
            PropertyNamingPolicy = new GithubJsonNaming(),
            WriteIndented = true
        };

        /// <summary>
        /// Converts source to JSON
        /// </summary>
        /// <typeparam name="T">Type of source</typeparam>
        /// <param name="source">Source</param>
        /// <returns></returns>
        internal static string ToJson<T>(this T source)
        {
            return JsonSerializer.Serialize(source, JsonOptions);
        }

        /// <summary>
        /// Converts JSON to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Desired type</typeparam>
        /// <param name="json">JSON</param>
        /// <returns></returns>
        internal static T FromJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentOutOfRangeException(nameof(json));
            }

            return JsonSerializer.Deserialize<T>(json, JsonOptions)
                ?? throw new ArgumentOutOfRangeException(nameof(json)); 
        }

        /// <summary>
        /// Splits camelCase to [camel,Case]
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns></returns>
        internal static string[] SplitByCamelCase(this string? text)
        {
            const string splitter = "\t";
            var parts = new List<string>();

            if (!string.IsNullOrEmpty(text))
            {
                var first = text[0].ToString();
                parts = text.Substring(1)
                    .Select((x, i) => i > 0 && char.IsUpper(x) ? splitter + x.ToString() : x.ToString())
                    .ToList();
                parts.Insert(0, first);
            }

            return string.Concat(parts).Split(splitter);
        }
    }
}
