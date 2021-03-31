using JgsReleases.Infrastructure.Exceptions;
using System;

namespace JgsReleases.Infrastructure.Builders
{
    /// <summary>
    /// Result of builders
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="isOk">Is ok result.</param>
        /// <param name="message">Result message</param>
        private Result(bool isOk, string message)
        {
            IsOK = isOk;
            Message = message;
        }

        /// <summary>
        /// Ctor. <see cref="IsOK"/> is set to <see langword="false"/> and message as
        /// <paramref name="exception"/>.Message
        /// </summary>
        /// <param name="exception">Exception occured</param>
        private Result(Exception? exception)
            : this(false, GetMessage(exception))
        {
            Exception = exception;
        }

        /// <summary>
        /// Is Ok result
        /// </summary>
        public bool IsOK { get; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Exception occured
        /// </summary>
        public Exception? Exception { get; }

#if NET5_0
        /// <summary>
        /// Creates ok result
        /// </summary>
        /// <param name="message">Custom message</param>
        /// <returns></returns>
        public static Result IsOk(string message = "") => new(true, message);

        /// <summary>
        /// Creates not ok result from occured exception
        /// </summary>
        /// <param name="exception">Occured exception</param>
        /// <returns></returns>
        public static Result WithError(Exception? exception) => new(exception);

        /// <summary>
        /// Creates not ok result with custom message
        /// </summary>
        /// <param name="message">Custom message</param>
        /// <returns></returns>
        public static Result WithError(string message) => new(false, message);
#else
        /// <summary>
        /// Created ok result
        /// </summary>
        /// <param name="message">Custom message</param>
        /// <returns></returns>
        public static Result IsOk(string message = "") => new Result(true, message);
        
        /// <summary>
        /// Created not ok result from occured exception
        /// </summary>
        /// <param name="exception">Occured exception</param>
        /// <returns></returns>
        public static Result WithError(Exception? exception) => new Result(exception);
        
        /// <summary>
        /// Creates not ok result with custom message
        /// </summary>
        /// <param name="message">Custom message</param>
        /// <returns></returns>
        public static Result WithError(string message) => new Result(false, message);
#endif

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"IsOK: {IsOK}, Message: {Message}";
        }

        /// <summary>
        /// Builds message from exception
        /// </summary>
        /// <param name="exception">Exception occured</param>
        /// <returns></returns>
        private static string GetMessage(Exception? exception)
        {
            string message;
            switch (exception)
            {
                case AggregateServerFileException agg:
                    message = agg.ToString();
                    break;
                case null:
                    message = string.Empty;
                    break;
                default:
                    message = exception.Message;
                    break;
            }

            return message;

        }
    }
}
