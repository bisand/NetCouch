using System.Net;
using System.Net.Http.Headers;

namespace NetCouch.Http
{
    public record ResponseData<T>(HttpHeaders Headers, HttpStatusCode StatusCode, string? StatusDescription, string? BodyString, T? Body)
    {
        public ResponseData() : this(new CouchDbHttpHeaders(), default, default, default, default)
        {
        }

        public string ContentType
        {
            get
            {
                if (Headers == null)
                    return string.Empty;

                if (Headers.TryGetValues("Content-Type", out var values))
                {
                    return values.FirstOrDefault() ?? string.Empty;
                }
                return string.Empty;
            }
            set
            {
                Headers.Remove("Content-Type");
                Headers.Add("Content-Type", value);
            }
        }
        public long ContentLength
        {
            get
            {
                if (Headers == null)
                    return 0;

                if (Headers.TryGetValues("Content-Length", out var values))
                {
                    return long.Parse(values.FirstOrDefault() ?? "0");
                }
                return 0;
            }
            set
            {
                Headers.Remove("Content-Length");
                Headers.Add("Content-Length", value.ToString());
            }
        }

        public dynamic DynamicData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(BodyString))
                    return default!;

                var body = System.Text.Json.JsonSerializer.Deserialize<dynamic>(BodyString);
                return body!;
            }
        }
    }

    public static class ResponseDataExtensions
    {
        public static ResponseData<T> Root<T>(this ResponseData<T> responseData)
        {
            return responseData;
        }
    }
}