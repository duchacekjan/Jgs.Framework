using System;

namespace JgsReleases.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception when processing file upload/download to server
    /// </summary>
    public class ServerFileException : Exception
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="fileName">Name of processed file</param>
        /// <param name="direction">File direction</param>
        /// <param name="innerException">Inner exception</param>
        public ServerFileException(string fileName, ServerFileDirection direction, Exception? innerException = null)
            : base(CreateMessage(fileName, direction), innerException)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Name of file
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Builds exception message based on file direction
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="direction">File Direction</param>
        /// <returns></returns>
        private static string? CreateMessage(string fileName, ServerFileDirection direction)
        {
            fileName = System.IO.Path.GetFileName(fileName);
            string format;
            switch (direction)
            {
                case ServerFileDirection.LocalFileNotExists:
                    format = "Local File '{0}' does not exists.";
                    break;
                case ServerFileDirection.UploadingFile:
                    format = "Could not upload file'{0}'.";
                    break;
                case ServerFileDirection.DownloadingFile:
                    format = "Could not download file'{0}'.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }

            return string.Format(format, fileName);
        }
    }
}
