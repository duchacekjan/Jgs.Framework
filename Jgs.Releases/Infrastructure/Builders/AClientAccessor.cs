using System;

namespace JgsReleases.Infrastructure.Builders
{
    /// <summary>
    /// Common accessor to client
    /// </summary>
    public abstract class AClientAccessor
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">Authorized client</param>
        internal AClientAccessor(GithubClient? client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            if (!Client.IsAuthorized)
            {
                throw new UnauthorizedAccessException("Unauthorized Github Client");
            }
        }

        /// <summary>
        /// Client
        /// </summary>
        private protected GithubClient Client { get; }
    }
}
