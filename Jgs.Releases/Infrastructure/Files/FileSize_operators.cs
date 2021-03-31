namespace JgsReleases.Infrastructure.Files
{
    /// <remarks>
    /// Operators for <see cref="FileSize"/>
    /// </remarks>
    public partial class FileSize
    {
#if NET5_0
        public static implicit operator FileSize(long bytes) => new(bytes);
#else
        public static implicit operator FileSize(long bytes) => new FileSize(bytes);
#endif

        public static implicit operator long(FileSize bytes) => bytes.B;

        public static FileSize operator /(FileSize a, FileSize b)
        {
            if (b.B == 0)
            {
                throw new System.DivideByZeroException();
            }

            return a.B / b.B;
        }

        public static FileSize operator %(FileSize a, FileSize b)
        {
            if (b.B == 0)
            {
                throw new System.DivideByZeroException();
            }

            return a.B % b.B;
        }

        public static FileSize operator *(FileSize a, FileSize b) => a.B * b.B;

        public static FileSize operator +(FileSize a) => a;

        public static FileSize operator -(FileSize a) => -a.B;

        public static FileSize operator +(FileSize a, FileSize b) => a.B + b.B;

        public static FileSize operator -(FileSize a, FileSize b) => a.B - b.B;

        public static bool operator ==(FileSize a, FileSize b) => a.B == b.B;

        public static bool operator !=(FileSize a, FileSize b) => a.B != b.B;

        public static bool operator >(FileSize a, FileSize b) => a.B > b.B;

        public static bool operator >=(FileSize a, FileSize b) => a.B >= b.B;

        public static bool operator <(FileSize a, FileSize b) => a.B < b.B;

        public static bool operator <=(FileSize a, FileSize b) => a.B <= b.B;

        public override bool Equals(object? obj)
        {
            bool result;
            if (ReferenceEquals(this, obj))
            {
                result = true;
            }
            else if (obj is null)
            {
                result = false;
            }
            else
            {
                switch (obj)
                {
                    case FileSize f:
                        result = B == f.B;
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            return B.GetHashCode();
        }

    }
}
