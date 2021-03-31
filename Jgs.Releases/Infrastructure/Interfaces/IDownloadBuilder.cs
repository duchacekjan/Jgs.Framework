using JgsReleases.Dto;

namespace JgsReleases.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for downloads
    /// </summary>
    public interface IDownloadBuilder
    {
        /// <summary>
        /// Download release assets
        /// </summary>
        /// <param name="assets">Array of release assets</param>
        /// <returns></returns>
        IAssetDownloadBuilder Assets(params AssetInfo[] assets);
    }
}
