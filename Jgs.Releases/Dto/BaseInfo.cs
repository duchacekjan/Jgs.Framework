namespace JgsReleases.Dto
{
    /// <summary>
    /// Base of info structures from GithubAPI
    /// </summary>
    public abstract class BaseInfo
    {
        /// <summary>
        /// Id of structure
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique node id
        /// </summary>
        public string NodeId { get; set; } = string.Empty;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
