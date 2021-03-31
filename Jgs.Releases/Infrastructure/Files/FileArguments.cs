using JgsReleases.Infrastructure.Progress;

namespace JgsReleases.Infrastructure.Files
{
    /// <summary>
    /// Arguments for file upload/download
    /// </summary>
    public sealed class FileArguments
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="route">Route for upload/download of file</param>
        /// <param name="fileName">Local file name. Is source(upload) or destination(download)</param>
        public FileArguments(string route, string fileName)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new System.ArgumentOutOfRangeException(nameof(route));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new System.ArgumentOutOfRangeException(nameof(fileName));
            }

            Route = route;
            FileName = fileName;
            CopyArguments = new CopyToArguments(new FileProgressArgs());
        }

        /// <summary>
        /// Route for upload/download of file
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// Local file name. Is source(upload) or destination(download)
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Arguments for <see cref="StreamExtensions.CopyToAsync(System.IO.Stream, System.IO.Stream, CopyToArguments)"/>
        /// </summary>
        public CopyToArguments CopyArguments { get; set; }
    }
}
