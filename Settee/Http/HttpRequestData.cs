namespace Biseth.Net.Settee.Http
{
    public class HttpRequestData
    {
        public HttpRequestData(string path, string query = null, string contentType = null, string method = HttpMethod.Get, string data = null)
        {
            Path = path;
            Query = query;
            ContentType = contentType;
            Method = method;
            Data = data;
        }

        public string Path { get; set; }
        public string ContentType { get; set; }
        public string Method { get; set; }
        public string Data { get; set; }
        public string Query { get; set; }
    }
}