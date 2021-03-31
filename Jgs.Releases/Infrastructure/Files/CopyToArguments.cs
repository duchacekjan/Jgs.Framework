using JgsReleases.Infrastructure.Progress;
using System.Threading;

namespace JgsReleases.Infrastructure.Files
{
    /// <summary>
    /// Arguments for <see cref="StreamExtensions.CopyToAsync(System.IO.Stream, System.IO.Stream, CopyToArguments)"/>
    /// </summary>
    public class CopyToArguments
    {
        public const int DefaultBufferSize = 4096;
        public const int MaxBufferSize = 1048576;
        private int m_bufferSize;
        private long? m_totalSize;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        public CopyToArguments(FileProgressArgs args, CancellationToken? token = null)
        {
            Token = token ?? CancellationToken.None;
            ProgressArguments = args;
            BufferSize = DefaultBufferSize;
        }

        /// <summary>
        /// Progress tracking arguments
        /// </summary>
        public FileProgressArgs ProgressArguments { get; }

        /// <summary>
        /// Buffer size
        /// </summary>
        public int BufferSize
        {
            get => m_bufferSize;
            set => SetBufferSize(value);
        }

        /// <summary>
        /// Total size. When not defined, used from source stream
        /// </summary>
        public long? TotalSize 
        {
            get => m_totalSize;
            set => SetTotalSize(value);
        }

        /// <summary>
        /// Cancellation token
        /// </summary>
        public CancellationToken Token { get; set; }

        /// <summary>
        /// Sets valid buffer size
        /// </summary>
        /// <param name="value">New value of buffer size</param>
        private void SetBufferSize(int value)
        {
            if (value <= 0)
            {
                value = DefaultBufferSize;
            }

            if (value > MaxBufferSize)
            {
                value = MaxBufferSize;
            }

            m_bufferSize = value;
        }

        /// <summary>
        /// Updates total size.
        /// </summary>
        /// <param name="value">New value</param>
        private void SetTotalSize(long? value)
        {
            m_totalSize = value;
            if (m_totalSize.HasValue)
            {
                ProgressArguments.Expected = m_totalSize.Value;
            }
        }
    }
}
