using System;

namespace JgsReleases.Infrastructure.Files
{
    /// <summary>
    /// Helper class for file upload/download speed
    /// </summary>
    public sealed class Speed
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="bytesPerSecond">Speed in bytes per second</param>
        public Speed(decimal bytesPerSecond)
        {
            Bps = bytesPerSecond;
        }

        /// <summary>
        /// Speed in Bytes/s
        /// </summary>
        public decimal Bps { get; set; }

        /// <summary>
        /// Speed in KiloBytes/s
        /// </summary>
        public decimal KBps => Math.Round(Bps / 1024, 2);

        /// <summary>
        /// Speed in MegaBytes/s
        /// </summary>
        public decimal MBps => Math.Round(Bps / 1024 / 1024, 2);
#if NET5_0
        public static implicit operator Speed(decimal bytesPerSecond) => new(bytesPerSecond);
#else
        public static implicit operator Speed(decimal bytesPerSecond) => new Speed(bytesPerSecond);
#endif
        /// <summary>
        /// Speed in Bytes/s with unit
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MBps > 1
                ? ToString(ByteSize.MB)
                : KBps > 1
                    ? ToString(ByteSize.KB)
                    : ToString(ByteSize.B);
        }

        /// <summary>
        /// Speed in desired unit with unit text
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <returns></returns>
        public string ToString(ByteSize unit)
        {
            string result;
            var numberFormat = $"{{0:#,0.00}} {unit}/s";
            switch (unit)
            {
                case ByteSize.MB:
                    result = string.Format(numberFormat, MBps);
                    break;
                case ByteSize.KB:
                    result = string.Format(numberFormat, KBps);
                    break;
                default:
                    result = string.Format(numberFormat, Bps);
                    break;
            }
            return result;
        }
    }
}
