using System;

namespace JgsReleases.Infrastructure.Exceptions
{
    /// <summary>
    /// Arguments validation exeption
    /// </summary>
    public class ArgumentsNotValidException
        : Exception
    {
        public ArgumentsNotValidException(string? argumentsName = null)
            : base($"'{argumentsName}' are not valid.")
        {
        }
    }
}
