namespace Biseth.Net.Settee.Http
{
    public class RequestData<T>
    {
        public RequestData(string url)
        {
            Url = url;
        }

        public RequestData(string url, T requestObject)
        {
            Url = url;
            RequestObject = requestObject;
        }

        public string Url { get; set; }
        public T RequestObject { get; set; }
    }
}