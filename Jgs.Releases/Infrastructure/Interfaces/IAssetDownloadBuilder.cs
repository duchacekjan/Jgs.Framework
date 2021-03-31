namespace JgsReleases.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for downloading release assets
    /// </summary>
    public interface IAssetDownloadBuilder : IResultBuilder<IAssetDownloadBuilder>
    {
        /// <summary>
        /// Custom target folder. If not defined, current directory is used
        /// </summary>
        /// <param name="folder">Path to download folder</param>
        /// <returns></returns>
        IAssetDownloadBuilder ToDownloadFolder(string folder);
    }
}
