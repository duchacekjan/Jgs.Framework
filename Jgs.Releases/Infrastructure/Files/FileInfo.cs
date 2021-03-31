namespace JgsReleases.Infrastructure.Files
{
    public class FileInfo
    {
        public string FileName { get; set; } = string.Empty;

        public int Index { get; set; }

        public int TotalFiles { get; set; }

        public override string ToString()
        {
            var onlyFileName = System.IO.Path.GetFileName(FileName);
            return $"{onlyFileName} ({Index}/{TotalFiles})";
        }
    }
}
