namespace JgsReleases.Infrastructure.Files
{
    /// <summary>
    /// Helper class for better work with file sizes
    /// </summary>
    public sealed partial class FileSize
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="bytes">File size in bytes</param>
        private FileSize(long bytes)
        {
            B = bytes;
        }

        /// <summary>
        /// Size in Bytes
        /// </summary>
        public long B { get; set; }

        /// <summary>
        /// Size in KiloBytes
        /// </summary>
        public decimal KB
        {
            get => System.Math.Round(B / 1024m, 2);
            set => B = (long)System.Math.Round(value * 1024);
        }

        /// <summary>
        /// Size in MegaBytes
        /// </summary>
        public decimal MB
        {
            get => System.Math.Round(B / 1024m / 1024m, 2);
            set => B = (long)System.Math.Round(value * 1024 * 1024);
        }

        /// <summary>
        /// Size in Bytes with unit
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MB > 1
                ? ToString(ByteSize.MB)
                : KB > 1
                    ? ToString(ByteSize.KB)
                    : ToString(ByteSize.B);
        }

        /// <summary>
        /// Size in desired unit with unit text;
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <returns></returns>
        public string ToString(ByteSize unit)
        {
            string result;
            var numberFormat = $"{{0:#,0}} {unit}";
            switch (unit)
            {
                case ByteSize.MB:
                    result = string.Format(numberFormat, MB);
                    break;
                case ByteSize.KB:
                    result = string.Format(numberFormat, KB);
                    break;
                default:
                    result = string.Format(numberFormat, B);
                    break;
            }
            return result;
        }
    }
}
