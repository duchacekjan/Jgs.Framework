namespace JgsReleases
{
    /// <summary>
    /// Key for creating release client
    /// </summary>
    public class Key
    {
        /// <summary>
        /// Github token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Repository name
        /// </summary>
        public string RepositoryName { get; set; } = string.Empty;

        /// <summary>
        /// Owner of repository
        /// </summary>
        public string Owner { get; set; } = string.Empty;
    }
}
