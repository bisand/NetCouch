using Biseth.Net.Settee.Serialization;

namespace Biseth.Net.Settee.Http
{
    public class RequestClient : IRequestClient
    {
        private IHttpClient _httpClient;
        private ISerializer _serializer;
        private string _url;

        public RequestClient(string url)
            : this(url, new HttpClient(), new NewtonSoftSerializer())
        {
        }

        public RequestClient(string url, IHttpClient httpClient, ISerializer serializer)
        {
            _url = url;
            _httpClient = httpClient;
            _serializer = serializer;
        }
    }
}