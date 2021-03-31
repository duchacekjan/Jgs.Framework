using JgsReleases.Infrastructure.Files;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JgsReleases.Infrastructure.Progress
{
    /// <summary>
    /// Custom http content supporting cancelling upload file with tracking progress
    /// </summary>
    internal class ProgressStreamContent : HttpContent
    {
        private readonly Stream m_content;
        private readonly CopyToArguments m_arguments;
        private bool m_contentConsumed;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="content">Content stream</param>
        /// <param name="arguments">Copy arguments</param>
        public ProgressStreamContent(Stream content, CopyToArguments arguments)
        {
            m_content = content ?? throw new ArgumentNullException(nameof(content));
            m_arguments = arguments;
        }

        /// <inheritdoc/>
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            Contract.Assert(stream != null);
            PrepareContent();
            await m_content.CopyToAsync(stream, m_arguments);
        }

        /// <inheritdoc/>
        protected override bool TryComputeLength(out long length)
        {
            length = m_content.Length;
            return true;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_content.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Content preparation
        /// </summary>
        private void PrepareContent()
        {
            if (m_contentConsumed)
            {
                // If the content needs to be written to a target stream a 2nd time, then the stream must support
                // seeking (e.g. a FileStream), otherwise the stream can't be copied a second time to a target 
                // stream (e.g. a NetworkStream).
                if (m_content.CanSeek)
                {
                    m_content.Position = 0;
                }
                else
                {
                    throw new InvalidOperationException("SR.net_http_content_stream_already_read");
                }
            }

            m_contentConsumed = true;
        }
    }
}