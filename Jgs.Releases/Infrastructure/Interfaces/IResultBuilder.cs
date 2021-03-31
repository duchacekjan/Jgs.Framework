using JgsReleases.Infrastructure.Builders;
using JgsReleases.Infrastructure.Progress;
using System.Threading;
using System.Threading.Tasks;

namespace JgsReleases.Infrastructure.Interfaces
{
    /// <summary>
    /// Base of builders
    /// </summary>
    public interface IResultBuilder
    {
        /// <summary>
        /// Starts processing
        /// </summary>
        /// <param name="bufferSize">Size of file chunks in bytes</param>
        /// <returns></returns>
        Task<Result> Start(int bufferSize = 4096);
    }

    /// <summary>
    /// Generic base of builders
    /// </summary>
    /// <typeparam name="T">Type of builder. Must be inherited from <see cref="IResultBuilder"/></typeparam>
    public interface IResultBuilder<T> : IResultBuilder
        where T : IResultBuilder
    {
        /// <summary>
        /// Track file download/upload progress
        /// </summary>
        /// <param name="progress">Method for tracking file download/upload progress</param>
        /// <returns></returns>
        T WithProgress(FileProgressHandler progress);

        /// <summary>
        /// Allow task cancellation with given token
        /// </summary>
        /// <param name="token">Token for cancelling task</param>
        /// <returns></returns>
        T WithCancellationToken(CancellationToken token);
    }
}
