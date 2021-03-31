using JgsReleases.Dto;

namespace JgsReleases.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface to start creating release
    /// </summary>
    public interface ICreateReleaseBuilder
    {
        /// <summary>
        /// Arguments for creating release
        /// </summary>
        /// <param name="arguments">Arguments</param>
        /// <returns></returns>
        IReleaseBuilder Release(NewReleaseArguments arguments);
    }
}
