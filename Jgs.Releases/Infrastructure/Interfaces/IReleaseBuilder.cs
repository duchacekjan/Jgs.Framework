using JgsReleases.Infrastructure.Progress;

namespace JgsReleases.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for creating release
    /// </summary>
    public interface IReleaseBuilder : IResultBuilder<IReleaseBuilder>
    {
        /// <summary>
        /// Track state changes
        /// </summary>
        /// <param name="stateChanged">Method to track state changes</param>
        /// <returns></returns>
        IReleaseBuilder WithTrackingStateChanges(StateChangedHandler stateChanged);

        /// <summary>
        /// Array of asset's file names to be uploaded to release
        /// </summary>
        /// <param name="assetsFileNames">array of asset's file names</param>
        /// <returns></returns>
        IReleaseBuilder WithAssets(params string[] assetsFileNames);
    }
}
