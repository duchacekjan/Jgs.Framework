namespace JgsReleases.Infrastructure.Exceptions
{
    /// <summary>
    /// Description what file has error
    /// </summary>
    public enum ServerFileDirection
    {
        /// <summary>
        /// Local file is non existing
        /// </summary>
        LocalFileNotExists,
        /// <summary>
        /// Uploading file
        /// </summary>
        UploadingFile,
        /// <summary>
        /// Downloading file
        /// </summary>
        DownloadingFile
    }
}
