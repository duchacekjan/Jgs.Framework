using System;
using System.Collections.Generic;

namespace JgsReleases.Infrastructure.Exceptions
{
    /// <summary>
    /// Agregate exception when uploading/downloading files
    /// </summary>
    public class AggregateServerFileException : AggregateException
    {
        /// <summary>
        /// Common base message. Used as title
        /// </summary>
        private static string CoreMessage => "Error in processing server files.";//TODO Resx

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="innerExceptions">List of inner exceptions</param>
        public AggregateServerFileException(IEnumerable<Exception> innerExceptions)
            : base(CoreMessage, innerExceptions)
        {
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(CoreMessage);
            foreach (var exception in InnerExceptions)
            {
                sb.AppendLine($"\t{exception.Message}");
            }

            return sb.ToString();
        }
    }
}
