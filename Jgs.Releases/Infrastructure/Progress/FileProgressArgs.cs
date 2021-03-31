﻿using JgsReleases.Infrastructure.Files;

namespace JgsReleases.Infrastructure.Progress
{
    /// <summary>
    /// Arguments for tracking file upload/download progress
    /// </summary>
    public class FileProgressArgs
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public FileProgressArgs()
        {
            Processed = 0;
            Expected = 0;
            HasError = false;
            Message = string.Empty;
            Speed = 0;
            FileInfo = new FileInfo();
            ReportProgress = delegate { };
        }

        /// <summary>
        /// Expected file size
        /// </summary>
        public FileSize Expected { get; set; }

        /// <summary>
        /// Information about file
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Processed file size
        /// </summary>
        public FileSize Processed { get; private set; }

        /// <summary>
        /// Current speed of upload/download
        /// </summary>
        public Speed Speed { get; private set; }

        /// <summary>
        /// Percentage of done
        /// </summary>
        public decimal Percentage { get; private set; }

        /// <summary>
        /// Has error
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// Custom message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Event to report progress
        /// </summary>
        public FileProgressHandler ReportProgress { get; set; }

        /// <summary>
        /// Set data when error occured
        /// </summary>
        /// <param name="message">Custom message</param>
        public void ProcessingError(string message = "")
        {
            Message = message;
            HasError = true;
            ReportProgress(this);
        }

        /// <summary>
        /// Set current state
        /// </summary>
        /// <param name="processed">Processed bytes</param>
        /// <param name="speed">Current peed</param>
        public void UpdateProgress(long processed, Speed speed)
        {
            Processed = processed;
            Speed = speed;
            Percentage = Processed / Expected;
            Message = string.Empty;
            ReportProgress(this);
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return HasError
                ? $"{FileInfo}\tERROR --> {Message}"
                : $"{FileInfo} --> {Processed}/{Expected}\t{Percentage:P} ({Speed})";
        }
    }
}