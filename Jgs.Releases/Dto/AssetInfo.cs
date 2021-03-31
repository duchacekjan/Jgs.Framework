using System;

namespace JgsReleases.Dto
{
    /// <summary>
    /// Release asset
    /// </summary>
    /// <example>
    ///{
    ///    "url": "https://apicom/repos/octocat/Hello-World/releases/assets/1",
    ///    "browser_download_url": "https://github.com/octocat/Hello-World/releases/download/v1.0.0/example.zip",
    ///    "id": 1,
    ///    "node_id": "MDEyOlJlbGVhc2VBc3NldDE=",
    ///    "name": "example.zip",
    ///    "label": "short description",
    ///    "state": "uploaded",
    ///    "content_type": "application/zip",
    ///    "size": 1024,
    ///    "download_count": 42,
    ///    "created_at": "2013-02-27T19:35:32Z",
    ///    "updated_at": "2013-02-27T19:35:32Z",
    ///    "uploader": {<see cref="BaseUser"/>}
    ///  }
    /// </example>
    public class AssetInfo : BaseInfo
    {
        /// <summary>
        /// Path to release asset, using download through GithubAPI
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Path to release asset, using download only in browser
        /// </summary>
        public string BrowserDownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// Asset label
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Content type of asset
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Size in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Number of downloads
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// DateTime, when created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// DateTime, when updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
