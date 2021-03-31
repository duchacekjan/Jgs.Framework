using JgsReleases.Infrastructure.Interfaces;
using JgsReleases.Infrastructure.Progress;
using System.Threading;
using System.Threading.Tasks;

namespace JgsReleases.Infrastructure.Builders
{
    /// <summary>
    /// Base builder
    /// </summary>
    public abstract class ResultBuilder<T> : AClientAccessor, IResultBuilder<T>
        where T : IResultBuilder
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">Authorized client</param>
        /// <param name="route">Common base route (if needed)</param>
        internal ResultBuilder(GithubClient? client, string route)
            : base(client)
        {
            Route = route;
            Progress = delegate { };
        }

        /// <summary>
        /// Common route
        /// </summary>
        protected string Route { get; }

        /// <summary>
        /// Size of buffer
        /// </summary>
        protected int BufferSize { get; private set; }

        /// <summary>
        /// Cancellation token
        /// </summary>
        protected CancellationToken Token { get; private set; }

        /// <summary>
        /// File progress tracking
        /// </summary>
        protected FileProgressHandler Progress { get; private set; }

        /// <summary>
        /// Starts processing
        /// </summary>
        /// <param name="bufferSize">BufferSize</param>
        /// <returns></returns>
        public async Task<Result> Start(int bufferSize = 4096)
        {
            BufferSize = bufferSize;
            return await Work();
        }

        /// <summary>
        /// Allow task cancellation with given token
        /// </summary>
        /// <param name="token">Token for cancelling task</param>
        /// <returns></returns>
        public T WithCancellationToken(CancellationToken token)
        {
            Token = token;
            return GetInstance();
        }

        /// <summary>
        /// Track file download/upload progress
        /// </summary>
        /// <param name="progress">Method for tracking file download/upload progress</param>
        /// <returns></returns>
        public T WithProgress(FileProgressHandler progress)
        {
            Progress = progress;
            return GetInstance();
        }

        /// <summary>
        /// Custom processing
        /// </summary>
        /// <returns></returns>
        protected abstract Task<Result> Work();

        /// <summary>
        /// Returns instance of <typeparamref name="T"/>
        /// </summary>
        /// <returns></returns>
        protected abstract T GetInstance();
    }
}
