using JgsReleases.Dto;
using JgsReleases.Infrastructure.Exceptions;
using JgsReleases.Infrastructure.Files;
using JgsReleases.Infrastructure.Interfaces;
using JgsReleases.Infrastructure.Progress;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JgsReleases.Infrastructure.Builders.Releases
{
    /// <summary>
    /// Builder for creating release
    /// </summary>
    public class ReleaseBuilder : ResultBuilder<IReleaseBuilder>, ICreateReleaseBuilder, IReleaseBuilder
    {
        private CreatingReleaseState m_state;
        private NewReleaseArguments m_arguments;
        private string[] m_assets = Array.Empty<string>();
        private StateChangedHandler m_stateChanged;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">Authorized client</param>
        /// <param name="releasesRoute">Route to repository's releases</param>
        internal ReleaseBuilder(GithubClient? client, string releasesRoute)
            : base(client, releasesRoute)
        {
            m_arguments = new NewReleaseArguments();
            m_stateChanged = delegate { };
            State = CreatingReleaseState.Initialized;
        }

        /// <summary>
        /// State of creation
        /// </summary>
        public CreatingReleaseState State
        {
            get => m_state;
            set => SetState(value);
        }

        /// <summary>
        /// Arguments for creating release
        /// </summary>
        /// <param name="arguments">Arguments</param>
        /// <returns></returns>
        public IReleaseBuilder Release(NewReleaseArguments arguments)
        {
            arguments.Validate();
            m_arguments = arguments;
            return this;
        }

        /// <summary>
        /// Track state changes
        /// </summary>
        /// <param name="stateChanged">Method to track state changes</param>
        /// <returns></returns>
        public IReleaseBuilder WithTrackingStateChanges(StateChangedHandler stateChanged)
        {
            m_stateChanged = stateChanged;
            return this;
        }

        /// <summary>
        /// Array of asset's file names to be uploaded to release
        /// </summary>
        /// <param name="assetsFileNames">array of asset's file names</param>
        /// <returns></returns>
        public IReleaseBuilder WithAssets(params string[] assetsFileNames)
        {
            m_assets = assetsFileNames;
            return this;
        }

        /// <summary>
        /// Returns instance of <see cref="IReleaseBuilder"/>
        /// </summary>
        /// <returns></returns>
        protected override IReleaseBuilder GetInstance() => this;

        /// <summary>
        /// Start of creating release
        /// </summary>
        /// <returns></returns>
        protected override async Task<Result> Work()
        {
            Result result;
            int? id = null;
            try
            {
                State = CreatingReleaseState.CreatingRelease;
                var release = await CreateReleaseBase();
                id = release.Id;
                await UploadAssets(release.UploadUrl);
                result = Result.IsOk($"Release '{m_arguments.Name}' successfully created");
                State = CreatingReleaseState.Success;
            }
            catch (Exception ex)
            {
                if (id.HasValue)
                {
                    State = CreatingReleaseState.ReleaseRollback;
                    await DeleteReleaseById(id.Value);
                }
                result = Result.WithError(ex);
                State = CreatingReleaseState.Error;
            }

            return result;
        }

        /// <summary>
        /// Formating route for upload url for asset
        /// </summary>
        /// <param name="baseUploadUrl">Base upload url for assets</param>
        /// <param name="assetFileName">Asset file name</param>
        /// <returns></returns>
        private static string GetAssetUploadUrl(string baseUploadUrl, string assetFileName)
        {
            var fileName = System.IO.Path.GetFileName(assetFileName);
            return baseUploadUrl.Replace("{?name,label}", $"?name={fileName}");
        }

        /// <summary>
        /// Deletes release by it's id.
        /// Serves for rollback
        /// </summary>
        /// <param name="id">Id of release</param>
        /// <returns></returns>
        private async Task DeleteReleaseById(int id)
        {
            try
            {
                var route = $"{Route}/{id}";
                await Client.DeleteAsync(route);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Creates new release without assets
        /// </summary>
        /// <param name="arguments">Arguments</param>
        /// <returns></returns>
        private async Task<ReleaseInfo> CreateReleaseBase()
        {
            ReleaseInfo result;
            try
            {
                m_arguments.Validate();
                result = await Client.PostAsync<NewReleaseArguments, ReleaseInfo>(Route, m_arguments);
            }
            catch (Exception ex)
            {
                throw new CreateReleaseException(ex);
            }

            return result;
        }

        /// <summary>
        /// Uploading assets
        /// </summary>
        /// <param name="uploadUrl">Base upload url for asset</param>
        /// <returns></returns>
        private async Task UploadAssets(string uploadUrl)
        {
            State = CreatingReleaseState.UploadingAssets;
            var errorList = new List<ServerFileException>();
            var index = 1;
            var count = m_assets.Length;
            foreach (var assetFileName in m_assets)
            {
                var fileInfo = new FileInfo
                {
                    FileName = assetFileName,
                    Index = index,
                    TotalFiles = count
                };
                var ex = await UploadAsset(uploadUrl, fileInfo);
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
        }

        /// <summary>
        /// Uploading single asset
        /// </summary>
        /// <param name="uploadUrl">Base upload url for asset</param>
        /// <param name="fileInfo">Information about file</param>
        /// <returns></returns>
        private async Task<ServerFileException?> UploadAsset(string uploadUrl, FileInfo fileInfo)
        {
            ServerFileException? result = null;
            var arg = new FileProgressArgs
            {
                FileInfo = fileInfo
            };

            try
            {
                if (System.IO.File.Exists(fileInfo.FileName))
                {
                    var route = GetAssetUploadUrl(uploadUrl, fileInfo.FileName);
                    var arguments = new FileArguments(route, fileInfo.FileName)
                    {
                        CopyArguments = new CopyToArguments(arg, Token)
                        {
                            BufferSize = BufferSize
                        }
                    };

                    await Client.UploadFileAsync<AssetInfo>(arguments);
                }
                else
                {
                    result = new ServerFileException(fileInfo.FileName, ServerFileDirection.LocalFileNotExists);
                    arg.ProcessingError(result.Message);
                    Progress(arg);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                arg.ProcessingError(ex.Message);
                Progress(arg);
                result = new ServerFileException(fileInfo.FileName, ServerFileDirection.UploadingFile, ex);
            }

            return result;
        }

        /// <summary>
        /// Sets creating state
        /// </summary>
        /// <param name="value">New state</param>
        private void SetState(CreatingReleaseState value)
        {
            if (value != m_state)
            {
                m_stateChanged(value, m_state);
            }

            m_state = value;
        }
    }
}
