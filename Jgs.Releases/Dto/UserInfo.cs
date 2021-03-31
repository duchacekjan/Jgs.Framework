namespace JgsReleases.Dto
{
    /// <summary>
    /// Simplified user info
    /// </summary>
    /// <example>
    ///{
    ///      "login": "octocat",
    ///      "id": 1,
    ///      "node_id": "MDQ6VXNlcjE=",
    ///      "avatar_url": "https://github.com/images/error/octocat_happy.gif",
    ///      "gravatar_id": "",
    ///      "url": "https://apicom/users/octocat",
    ///      "html_url": "https://github.com/octocat",
    ///      "followers_url": "https://apicom/users/octocat/followers",
    ///      "following_url": "https://apicom/users/octocat/following{/other_user}",
    ///      "gists_url": "https://apicom/users/octocat/gists{/gist_id}",
    ///      "starred_url": "https://apicom/users/octocat/starred{/owner}{/repo}",
    ///      "subscriptions_url": "https://apicom/users/octocat/subscriptions",
    ///      "organizations_url": "https://apicom/users/octocat/orgs",
    ///      "repos_url": "https://apicom/users/octocat/repos",
    ///      "events_url": "https://apicom/users/octocat/events{/privacy}",
    ///      "received_events_url": "https://apicom/users/octocat/received_events",
    ///      "type": "User",
    ///      "site_admin": false
    ///    }
    /// </example>
    public class UserInfo
    {
        /// <summary>
        /// Id of user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Login name of user
        /// </summary>
        public string Login { get; set; } = string.Empty;
    }
}
