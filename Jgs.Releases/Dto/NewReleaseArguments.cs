using JgsReleases.Infrastructure.Exceptions;

namespace JgsReleases.Dto
{
    /// <summary>
    /// Structure for creating new release through Github API
    /// </summary>
    public class NewReleaseArguments
    {
        /// <summary>
        /// Name of tag for release (required argument)
        /// </summary>
        public string TagName { get; set; } = string.Empty;

        /// <summary>
        /// Target branch of release. Default is 'master' (required argument)
        /// </summary>
        public string TargetCommitish { get; set; } = "master";

        /// <summary>
        /// Name of release (required argument)
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of release
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        /// Is draft
        /// </summary>
        public bool Draft { get; set; }

        /// <summary>
        /// Is prerelease
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Method to validate data
        /// </summary>
        /// <exception cref="ArgumentsNotValidException">When missing required data</exception>
        public void Validate()
        {
            if (!GetIsValid())
            {
                throw new ArgumentsNotValidException(nameof(NewReleaseArguments));
            }
        }

        /// <summary>
        /// Method returns if data are valid
        /// </summary>
        /// <returns></returns>
        private bool GetIsValid()
        {
            return !string.IsNullOrEmpty(TagName)
                && !string.IsNullOrEmpty(TargetCommitish)
                && !string.IsNullOrEmpty(Name);
        }
    }
}
