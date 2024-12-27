using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace NetCouch.Http
{
    public class RequestClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializationOptions;
        private bool _disposed;
        private string _url;

        public RequestClient(string url, string? username = null, string? password = null)
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5), // Refresh connections periodically
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 10, // Adjust based on expected load
            };

            _url = url;
            _httpClient = new HttpClient(handler);
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
            _serializationOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        private static async Task<ResponseData<TOut>> GetResponseDataAsync<TOut>(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();
            var length = response.Content.Headers.ContentLength ?? 0;
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/json";
            var body = JsonSerializer.Deserialize<TOut>(data);
            return new ResponseData<TOut> { BodyString = data, Body = body, ContentLength = length, ContentType = contentType, StatusCode = response.StatusCode, StatusDescription = response.ReasonPhrase ?? string.Empty };
        }

        public async Task<ResponseData<TOut>> GetAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            var url = new Uri(new Uri(_url), path);
            var response = await _httpClient.GetAsync(url);
            return await GetResponseDataAsync<TOut>(response);
        }

        public ResponseData<TOut> Get<TOut>(string path)
        {
            return GetAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> HeadAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var requestData = new RequestData<TOut>(path);
            var httpRequestData = new HttpRequestMessage(HttpMethod.Head, path);
            var response = await _httpClient.SendAsync(httpRequestData);
            return await GetResponseDataAsync<TOut>(response);
        }

        public ResponseData<TOut> Head<TOut>(string path)
        {
            return HeadAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> OptionsAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var requestData = new RequestData<TOut>(path);
            var httpRequestData = new HttpRequestMessage(HttpMethod.Options, path);
            var response = await _httpClient.SendAsync(httpRequestData);
            return await GetResponseDataAsync<TOut>(response);
        }

        public ResponseData<TOut> Options<TOut>(string path)
        {
            return OptionsAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> DeleteAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var url = new Uri(new Uri(_url), path);
            var response = await _httpClient.DeleteAsync(url);
            return await GetResponseDataAsync<TOut>(response);
        }

        public ResponseData<TOut> Delete<TOut>(string path)
        {
            return DeleteAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> PutAsync<TIn, TOut>(RequestData<TIn> requestData)
        {
            ArgumentNullException.ThrowIfNull(requestData, nameof(requestData));
            var jsonBody = JsonSerializer.Serialize(requestData.Body, _serializationOptions);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            foreach (var header in requestData?.Headers ?? new CustomHttpHeaders())
            {
                content.Headers.Add(header.Key, header.Value);
            }
            var url = new Uri(new Uri(_url), requestData?.Url);
            var response = await _httpClient.PutAsync(url, content);
            return await GetResponseDataAsync<TOut>(response);
        }

        public ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn> requestData)
        {
            return PutAsync<TIn, TOut>(requestData).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> PostAsync<TIn, TOut>(RequestData<TIn> requestData)
        {
            ArgumentNullException.ThrowIfNull(requestData, nameof(requestData));
            var jsonBody = JsonSerializer.Serialize(requestData.Body, _serializationOptions);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            foreach (var header in requestData?.Headers ?? new CustomHttpHeaders())
            {
                content.Headers.Add(header.Key, header.Value);
            }
            var url = new Uri(new Uri(_url), requestData?.Url);
            var response = await _httpClient.PostAsync(url, content);
            return await GetResponseDataAsync<TOut>(response);
        }

        public ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData)
        {
            return PostAsync<TIn, TOut>(requestData).GetAwaiter().GetResult();
        }

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
            }
            _disposed = true;
        }

        ~RequestClient()
        {
            Dispose(false);
        }

        #endregion
    }
}