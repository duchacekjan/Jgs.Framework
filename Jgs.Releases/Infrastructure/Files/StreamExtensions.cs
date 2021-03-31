using System;
using System.IO;
using System.Threading.Tasks;

namespace JgsReleases.Infrastructure.Files
{
    /// <summary>
    /// Extensions for stream
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Copying data from one stream to another with chunks defined in <paramref name="arguments"/>
        /// </summary>
        /// <param name="source">Source stream</param>
        /// <param name="destination">Destination stream</param>
        /// <param name="arguments">Arguments of copying</param>
        /// <returns></returns>
        public static async Task CopyToAsync(this Stream source, Stream destination, CopyToArguments arguments)
        {
            try
            {
                await Task.Run(() =>
                {
                    var buffer = new byte[arguments.BufferSize];
                    var size = arguments.TotalSize ?? source.Length;
                    arguments.TotalSize = size;
                    var processed = 0L;
                    var sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    using (source)
                    {
                        var length = 0;
                        do
                        {
                            length = source.Read(buffer, 0, buffer.Length);
                            if (length > 0)
                            {
                                processed += length;
                                var speed = processed.CalculateSpeed(sw.ElapsedMilliseconds);
                                arguments.ProgressArguments.UpdateProgress(processed, speed);
                                destination.Write(buffer, 0, length);
                            }
                        } while ((length > 0) && !arguments.Token.IsCancellationRequested);
                    }
                });

                arguments.Token.ThrowIfCancellationRequested();

            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        /// <summary>
        /// Calculates rough speed based on processed bytes and ellapsed time in ms
        /// </summary>
        /// <param name="processed">Total processed size</param>
        /// <param name="milliseconds">Ellapsed time (ms)</param>
        /// <returns></returns>
        private static Speed CalculateSpeed(this long processed, long milliseconds)
        {
            Speed result = 0;
            if (milliseconds > 0)
            {
                result = Math.Round((processed * 1000m) / milliseconds, 2);
            }

            return result;
        }
    }
}
