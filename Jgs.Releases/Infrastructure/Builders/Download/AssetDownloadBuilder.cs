using JgsReleases.Dto;
using JgsReleases.Infrastructure.Exceptions;
using JgsReleases.Infrastructure.Files;
using JgsReleases.Infrastructure.Interfaces;
using JgsReleases.Infrastructure.Progress;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JgsReleases.Infrastructure.Builders.Download
{
    /// <summary>
    /// Builder for downloading release assets
    /// </summary>
    public class AssetDownloadBuilder : ResultBuilder<IAssetDownloadBuilder>, IAssetDownloadBuilder
    {
        private readonly AssetInfo[] m_assets;
        private string m_destinationFolder;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">Authorized client</param>
        /// <param name="assets">Assets to download</param>
        internal AssetDownloadBuilder(GithubClient? client, AssetInfo[] assets)
            : base(client, string.Empty)
        {
            m_assets = assets;
            m_destinationFolder = string.Empty;
        }

        /// <summary>
        /// Custom target folder. If not defined, current directory is used
        /// </summary>
        /// <param name="folder">Path to download folder</param>
        /// <returns></returns>
        public IAssetDownloadBuilder ToDownloadFolder(string folder)
        {
            m_destinationFolder = folder;
            return this;
        }

        /// <summary>
        /// Returns instance of <see cref="IAssetDownloadBuilder"/>
        /// </summary>
        /// <returns></returns>
        protected override IAssetDownloadBuilder GetInstance() => this;

        /// <summary>
        /// Start of downloading of assets
        /// </summary>
        /// <returns></returns>
        protected override async Task<Result> Work()
        {
            Result result;
            if (m_assets.Length == 0)
            {
                result = Result.IsOk($"No assets to download");
            }
            else
            {
                result = await DownloadAssets();
            }

            return result;
        }

        /// <summary>
        /// Method downloads asset to given folder
        /// </summary>
        /// <returns></returns>
        private async Task<Result> DownloadAssets()
        {
            Result result;
            try
            {
                if (string.IsNullOrEmpty(m_destinationFolder))
                {
                    m_destinationFolder = System.IO.Directory.GetCurrentDirectory();
                }

                if (!System.IO.Directory.Exists(m_destinationFolder))
                {
                    System.IO.Directory.CreateDirectory(m_destinationFolder);
                }

                var errorList = new List<ServerFileException>();
                var index = 1;
                var count = m_assets.Length;
                foreach (var asset in m_assets)
                {
                    var ex = await DownloadAsset(asset, index, count);
                    if (ex != null)
                    {
                        errorList.Add(ex);
                    }
                    index++;
                }

                if (errorList.Count > 0)
                {
                    throw new AggregateServerFileException(errorList);
                }

                result = Result.IsOk($"All assets downloaded to '{m_destinationFolder}'");
            }
            catch (System.Exception ex)
            {
                result = Result.WithError(ex);
            }

            return result;
        }

        /// <summary>
        /// Method downloads one asset
        /// </summary>
        /// <param name="asset">Asset info</param>
        /// <param name="arg">File progress arguments</param>
        /// <returns></returns>
        private async Task<ServerFileException?> DownloadAsset(AssetInfo asset, int fileIndex, int fileCount)
        {
            ServerFileException? result = null;
            var fileName = GetTargetFileName(asset);
            var arg = new FileProgressArgs
            {
                FileInfo = new FileInfo
                {
                    FileName = fileName,
                    Index = fileIndex,
                    TotalFiles = fileCount
                },
                ReportProgress = Progress
            };
            try
            {
                var file = new FileArguments(asset.Url, fileName)
                {
                    CopyArguments = new CopyToArguments(arg, Token)
                    {
                        BufferSize = BufferSize
                    }
                };
                await Client.DownloadFileAsync(file);
            }
            catch (System.OperationCanceledException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                arg.ProcessingError(ex.Message);
                Progress(arg);
                result = new ServerFileException(fileName, ServerFileDirection.DownloadingFile, ex);
            }
            return result;
        }

        /// <summary>
        /// Builds target file name from asset info
        /// </summary>
        /// <param name="asset">Asset info</param>
        /// <returns></returns>
        private string GetTargetFileName(AssetInfo asset)
        {
            return System.IO.Path.Combine(m_destinationFolder, asset.Name);
        }
    }
}
