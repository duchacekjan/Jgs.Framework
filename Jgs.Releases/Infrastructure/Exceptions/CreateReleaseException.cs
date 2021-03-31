using System;
namespace JgsReleases.Infrastructure.Exceptions
{
    /// <summary>
    /// Error occured during creation of release
    /// </summary>
    public class CreateReleaseException : Exception
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="innerException">Exception caused error</param>
        public CreateReleaseException(Exception? innerException) 
            : base("Could not create release.", innerException)
        {
        }
    }
}
