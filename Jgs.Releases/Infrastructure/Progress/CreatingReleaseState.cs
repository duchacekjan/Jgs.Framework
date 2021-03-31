namespace JgsReleases.Infrastructure.Progress
{
    /// <summary>
    /// State of release create process
    /// </summary>
    public enum CreatingReleaseState
    {
        Initialized,
        CreatingRelease,
        UploadingAssets,
        Success,
        Error,
        ReleaseRollback
    }
}
