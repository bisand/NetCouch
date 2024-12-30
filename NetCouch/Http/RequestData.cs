using System.Net;
using System.Net.Http.Headers;

namespace NetCouch.Http;

public class RequestData<T>(string url, T? body = default, string? contentType = default)
{
    public string Url { get; set; } = url;
    public string? ContentType { get; set; } = contentType;
    public HttpHeaders? Headers { get; set; } = new CouchDbHttpHeaders();
    public T? Body { get; set; } = body;
}