using JgsReleases.Dto;
using JgsReleases.Infrastructure.Interfaces;

namespace JgsReleases.Infrastructure.Builders.Download
{
    /// <summary>
    /// Builder for downloads
    /// </summary>
    public class DownloadBuilder : AClientAccessor, IDownloadBuilder
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="client">Authorized client</param>
        internal DownloadBuilder(GithubClient? client)
            : base(client)
        {
        }

        /// <summary>
        /// Method for downloading release assets
        /// </summary>
        /// <param name="assets">Array of release assets</param>
        /// <returns></returns>
        public IAssetDownloadBuilder Assets(AssetInfo[] assets) => new AssetDownloadBuilder(Client, assets);
    }
}
