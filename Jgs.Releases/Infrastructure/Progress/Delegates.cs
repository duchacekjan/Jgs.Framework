namespace JgsReleases.Infrastructure.Progress
{
    /// <summary>
    /// Tracking progress of file download/upload
    /// </summary>
    /// <param name="args">Arguments</param>
    public delegate void FileProgressHandler(FileProgressArgs args);

    /// <summary>
    /// Tracking <see cref="CreatingReleaseState"/> changes
    /// </summary>
    /// <param name="newValue">New state</param>
    /// <param name="oldValue">Old state</param>
    public delegate void StateChangedHandler(CreatingReleaseState newValue, CreatingReleaseState oldValue);
}
