namespace Biseth.Net.Settee.Http
{
    public class HttpRequestData
    {
        public HttpRequestData(string path, string contentType = null, string method = HttpMethod.Get, string data = null)
        {
            Path = path;
            ContentType = contentType;
            Method = method;
            Data = data;
        }

        public string Path { get; set; }
        public string ContentType { get; set; }
        public string Method { get; set; }
        public string Data { get; set; }
    }
}