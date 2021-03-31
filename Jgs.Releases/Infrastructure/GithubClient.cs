using JgsReleases.Dto;
using JgsReleases.Infrastructure.Files;
using JgsReleases.Infrastructure.Progress;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JgsReleases.Infrastructure
{
    /// <summary>
    /// Github client to communicate Github API
    /// </summary>
    internal class GithubClient
    {
        public const string GitHubApiUrl = "https://apicom";

        private readonly ProductInfoHeaderValue m_userAgent;
#if NET5_0
        private readonly MediaTypeWithQualityHeaderValue m_jsonContent = new("application/json");
        private readonly MediaTypeWithQualityHeaderValue m_streamContent = new("application/octet-stream");
#else
        private readonly MediaTypeWithQualityHeaderValue m_jsonContent = 
            new MediaTypeWithQualityHeaderValue("application/json");
        private readonly MediaTypeWithQualityHeaderValue m_streamContent = 
            new MediaTypeWithQualityHeaderValue("application/octet-stream");
#endif

        private HttpClient? m_client;

        /// <summary>
        /// Ctor.
        /// </summary>
        public GithubClient()
        {
            m_userAgent = new ProductInfoHeaderValue(GetType().Name, "1.0");
        }

        /// <summary>
        /// Client is authorized
        /// </summary>
        public bool IsAuthorized { get; private set; }

        /// <summary>
        /// Information about logged user
        /// </summary>
        public UserInfo? User { get; private set; }

        /// <summary>
        /// Login by user name and token
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public async Task Login(string userName, string token)
        {
            try
            {
                m_client = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("GET"), $"{GitHubApiUrl}/user");
                request.Headers.UserAgent.Add(m_userAgent);
                var base64authorization = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{userName}:{token}"));
                request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                User = await SendAsync<UserInfo>(m_client, request);
                m_client = GetAuthorizedClient(token);
                IsAuthorized = User != null && User.Login == userName;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await Logout();
            }
        }

        /// <summary>
        /// Logout currently logged user
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            User = null;
            m_client?.Dispose();
            m_client = null;
            await Task.CompletedTask;
            IsAuthorized = false;
        }

        /// <summary>
        /// Method to call HTTP GET method for given url
        /// </summary>
        /// <typeparam name="T">Type of returned object</typeparam>
        /// <param name="url">Route</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await SendAsync<T>(m_client, request);
        }

        /// <summary>
        /// Method to call HTTP POST with givent content
        /// </summary>
        /// <typeparam name="TContent">Type of content</typeparam>
        /// <typeparam name="TResponse">Type of returned object</typeparam>
        /// <param name="url">Route</param>
        /// <param name="content">Content</param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TContent, TResponse>(string url, TContent content)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            var stringContent = new StringContent(content.ToJson());
            stringContent.Headers.ContentType = m_jsonContent;
            request.Content = stringContent;
            return await SendAsync<TResponse>(m_client, request);
        }

        /// <summary>
        /// Method to call HTTP DELETE with given url
        /// </summary>
        /// <param name="url">Route</param>
        /// <returns></returns>
        public async Task DeleteAsync(string url)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            await SendAsync(m_client, request);
        }

        /// <summary>
        /// Method for uploading file with HTTP POST
        /// </summary>
        /// <typeparam name="T">Type of returned object</typeparam>
        /// <param name="arguments">File arguments</param>
        /// <returns></returns>
        public async Task<T> UploadFileAsync<T>(FileArguments arguments)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, arguments.Route);
            using var file = System.IO.File.Open(arguments.FileName, System.IO.FileMode.Open);
            var streamContent = new ProgressStreamContent(file, arguments.CopyArguments);
            streamContent.Headers.ContentType = m_streamContent;
            request.Content = streamContent;
            return await SendAsync<T>(m_client, request);
        }

        /// <summary>
        /// Method for downloading file with HTTP GET
        /// </summary>
        /// <param name="arguments">File arguments</param>
        /// <returns></returns>
        public async Task DownloadFileAsync(FileArguments arguments)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, arguments.Route);
            request.Headers.Accept.Add(m_streamContent);
            var tempFileName = System.IO.Path.GetTempFileName();
            using var file = System.IO.File.Open(tempFileName, System.IO.FileMode.OpenOrCreate);
            var response = await SendAsync(m_client, request, HttpCompletionOption.ResponseHeadersRead);
            var source = await response.Content.ReadAsStreamAsync();
            arguments.CopyArguments.TotalSize = response.Content.Headers.ContentLength;
            try
            {
                await source.CopyToAsync(file, arguments.CopyArguments);
                file.Close();
                System.IO.File.Move(tempFileName, arguments.FileName, true);
            }
            catch (System.OperationCanceledException)
            {
                file.Close();
                System.IO.File.Delete(tempFileName);
                throw;
            }
        }

        /// <summary>
        /// Sends request to client
        /// </summary>
        /// <typeparam name="T">Type of returned object</typeparam>
        /// <param name="client">Client use for sending</param>
        /// <param name="request">Request</param>
        /// <param name="completition">Request completition</param>
        /// <returns></returns>
        private static async Task<T> SendAsync<T>(HttpClient? client, HttpRequestMessage request, HttpCompletionOption completition = HttpCompletionOption.ResponseContentRead)
        {
            var response = await SendAsync(client, request, completition);
            response.EnsureSuccessStatusCode();
            return await ProcessJsonResponse<T>(response);
        }

        /// <summary>
        /// Sends request to client
        /// </summary>
        /// <param name="client">Client use for sending</param>
        /// <param name="request">Request</param>
        /// <param name="completition">Request completition</param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> SendAsync(HttpClient? client, HttpRequestMessage request, HttpCompletionOption completition = HttpCompletionOption.ResponseContentRead)
        {
            if (client == null)
            {
                throw new System.ArgumentNullException(nameof(client));
            }

            var response = await client.SendAsync(request, completition);
            response.EnsureSuccessStatusCode();

            return response;
        }

        /// <summary>
        /// Method to process JSON response and returning correct object of given type
        /// </summary>
        /// <typeparam name="T">Type of returned object</typeparam>
        /// <param name="response">Http response</param>
        /// <returns></returns>
        private static async Task<T> ProcessJsonResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content.FromJson<T>();
        }

        /// <summary>
        /// Creates authorized client with correct headers
        /// </summary>
        /// <param name="token">Github Token</param>
        /// <param name="handler">Custom handler</param>
        /// <returns></returns>
        private HttpClient GetAuthorizedClient(string token, HttpClientHandler? handler = null)
        {
            var client = handler != null
                ? new HttpClient(handler)
                : new HttpClient();

            client.BaseAddress = new System.Uri(GitHubApiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vndv3+json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            client.DefaultRequestHeaders.UserAgent.Add(m_userAgent);
            return client;
        }
    }
}
