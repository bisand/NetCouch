using System.Net;

namespace Biseth.Net.Couch.Http
{
    public class RequestData<T>
    {
        public RequestData(string url)
        {
            Url = url;
        }

        public RequestData(string url, T requestObject, string contentType, WebHeaderCollection headers = null)
        {
            Url = url;
            RequestObject = requestObject;
            ContentType = contentType;
            Headers = headers;
        }

        public string Url { get; set; }
        public string ContentType { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public T RequestObject { get; set; }
    }
}