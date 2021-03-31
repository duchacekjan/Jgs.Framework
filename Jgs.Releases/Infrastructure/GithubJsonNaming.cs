using System.Text.Json;

namespace JgsReleases.Infrastructure
{
    /// <summary>
    /// Github api naming policy
    /// </summary>
    public class GithubJsonNaming : JsonNamingPolicy
    {
        /// <summary>
        /// Converts property in PascalCaseName to pascal_case_name
        /// </summary>
        public override string ConvertName(string name)
        {
            var result = name;
            if (!string.IsNullOrEmpty(name))
            {
                var parts = name.SplitByCamelCase();
                result = string.Join("_", parts).ToLower();
            }
            return result;
        }
    }
}
