using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetCouch.Http;

public class CouchDbClient : IRequestClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializationOptions;
    private bool _disposed;
    private string _url;

    public CouchDbClient(string url, string? username = null, string? password = null)
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

    private async Task<ResponseData<TOut>> GetResponseDataAsync<TOut>(HttpResponseMessage response)
    {
        var data = await response.Content.ReadAsStringAsync();
        var length = response.Content.Headers.ContentLength ?? 0;
        var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/json";
        var body = JsonSerializer.Deserialize<TOut>(data, _serializationOptions);
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
            throw new ArgumentNullException(nameof(path));

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
            throw new ArgumentNullException(nameof(path));

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
            throw new ArgumentNullException(nameof(path));

        var url = new Uri(new Uri(_url), path);
        var response = await _httpClient.DeleteAsync(url);
        return await GetResponseDataAsync<TOut>(response);
    }

    public ResponseData<TOut> Delete<TOut>(string path)
    {
        return DeleteAsync<TOut>(path).GetAwaiter().GetResult();
    }

    public async Task<ResponseData<TOut>> SendAsync<TIn, TOut>(RequestData<TIn>? requestData, HttpMethod method)
    {
        ArgumentNullException.ThrowIfNull(requestData, nameof(requestData));

        // Serialize request body
        var jsonBody = JsonSerializer.Serialize(requestData.Body, _serializationOptions);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Prepare the request
        var url = new Uri(new Uri(_url), requestData?.Url);
        var request = new HttpRequestMessage(method, url)
        {
            Content = content
        };

        // Add headers to the request
        foreach (var header in requestData?.Headers ?? new CouchDbHttpHeaders())
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        // Send the request
        var response = await _httpClient.SendAsync(request);
        return await GetResponseDataAsync<TOut>(response);
    }

    public ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn>? requestData)
    {
        return SendAsync<TIn, TOut>(requestData, HttpMethod.Put).GetAwaiter().GetResult();
    }

    public ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData)
    {
        return SendAsync<TIn, TOut>(requestData, HttpMethod.Post).GetAwaiter().GetResult();
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

    ~CouchDbClient()
    {
        Dispose(false);
    }

    #endregion
}
