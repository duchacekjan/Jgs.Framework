using JgsReleases.Dto;
using JgsReleases.Infrastructure;
using JgsReleases.Infrastructure.Builders.Download;
using JgsReleases.Infrastructure.Builders.Releases;
using JgsReleases.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JgsReleases
{
    /// <summary>
    /// Client for working with releases in Github
    /// </summary>
    public class ReleasesClient
    {
        private readonly Key m_key;
        private readonly GithubClient m_client;

        /// <summary>
        /// Key in form of JSON
        /// </summary>
        /// <param name="key">Key</param>
        public ReleasesClient(string key)
            : this(ToKey(key))
        {
        }

        /// <summary>
        /// Key in form of class
        /// </summary>
        /// <param name="key">Key</param>
        public ReleasesClient(Key key)
        {
            m_key = key;
            m_client = new GithubClient();
        }

        /// <summary>
        /// Key in form of arguments
        /// </summary>
        /// <param name="owner">Repository owner</param>
        /// <param name="repo">Repository name</param>
        /// <param name="token">Github token</param>
        public ReleasesClient(string owner, string repo, string token)
            : this(new Key { Owner = owner, RepositoryName = repo, Token = token })
        {
        }

        /// <summary>
        /// Start of release creation
        /// </summary>
        public ICreateReleaseBuilder Create => new ReleaseBuilder(m_client, ReleasesRoute);

        /// <summary>
        /// Start of file(s) download
        /// </summary>
        public IDownloadBuilder Download => new DownloadBuilder(m_client);

        /// <summary>
        /// Route to releases
        /// </summary>
        private string ReleasesRoute => $"/repos/{m_key.Owner}/{m_key.RepositoryName}/releases";

        /// <summary>
        /// Login to repository
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            await m_client.Login(m_key.Owner, m_key.Token);
        }

        /// <summary>
        /// Logout from repository
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            if (m_client.IsAuthorized)
            {
                await m_client.Logout();
            }
        }

        /// <summary>
        /// Gets all releases from repo
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReleaseInfo>> GetReleasesAsync() //TODO PAGING
        {
            List<ReleaseInfo> result;
            try
            {
                var allreleases = await m_client.GetAsync<List<ReleaseInfo>>(ReleasesRoute);
                var latest = await m_client.GetAsync<ReleaseInfo>($"{ReleasesRoute}/latest");
                result = allreleases
                    .Select(s =>
                    {
                        s.IsLatest = latest.Id == s.Id;
                        return s;
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = new List<ReleaseInfo>();
            }

            return result;
        }

        /// <summary>
        /// Converts JSON key to <see cref="Key"/>
        /// </summary>
        /// <param name="key">JSON</param>
        /// <returns></returns>
        private static Key ToKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            var json = Convert.FromBase64String(key);
            return System.Text.Json.JsonSerializer.Deserialize<Key>(json)
                ?? throw new ArgumentOutOfRangeException(nameof(key));
        }
    }
}
