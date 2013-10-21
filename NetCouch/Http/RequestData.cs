namespace Biseth.Net.Couch.Http
{
    public class RequestData<T>
    {
        public RequestData(string url)
        {
            Url = url;
        }

        public RequestData(string url, T requestObject, string contentType)
        {
            Url = url;
            RequestObject = requestObject;
            ContentType = contentType;
        }

        public string Url { get; set; }
        public string ContentType { get; set; }
        public T RequestObject { get; set; }
    }
}