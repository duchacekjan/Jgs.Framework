using System;

namespace JgsReleases.Dto
{
    /// <summary>
    /// Release structure
    /// </summary>
    /// <example>
    /// {
    ///"url": "https://apicom/repos/octocat/Hello-World/releases/1",
    ///"html_url": "https://github.com/octocat/Hello-World/releases/v1.0.0",
    ///"assets_url": "https://apicom/repos/octocat/Hello-World/releases/1/assets",
    ///"upload_url": "https://uploadscom/repos/octocat/Hello-World/releases/1/assets{?name,label}",
    ///"tarball_url": "https://apicom/repos/octocat/Hello-World/tarball/v1.0.0",
    ///"zipball_url": "https://apicom/repos/octocat/Hello-World/zipball/v1.0.0",
    ///"id": 1,
    ///"node_id": "MDc6UmVsZWFzZTE=",
    ///"tag_name": "v1.0.0",
    ///"target_commitish": "master",
    ///"name": "v1.0.0",
    ///"body": "Description of the release",
    ///"draft": false,
    ///"prerelease": false,
    ///"created_at": "2013-02-27T19:35:32Z",
    ///"published_at": "2013-02-27T19:35:32Z",
    ///"author": {<see cref="UserInfo"/>}
    /// </example>
    public class ReleaseInfo : BaseInfo
    {
        /// <summary>
        /// Name of release tag
        /// </summary>
        public string TagName { get; set; } = string.Empty;

        /// <summary>
        /// Target branch of release
        /// </summary>
        public string TargetCommitish { get; set; } = string.Empty;

        /// <summary>
        /// Is draft
        /// </summary>
        public bool Draft { get; set; }

        /// <summary>
        /// Is prerelease
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// DateTime, when created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// DateTime, when published
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Array of release assets
        /// </summary>
        public AssetInfo[] Assets { get; set; } = Array.Empty<AssetInfo>();

        /// <summary>
        /// Url for uploading assets
        /// </summary>
        public string UploadUrl { get; set; } = string.Empty;

        /// <summary>
        /// Is marked as latest
        /// </summary>
        /// <remarks>
        /// Not part of original GithubAPI structure. Custom extension
        /// </remarks>
        public bool IsLatest { get; set; }
    }
}
