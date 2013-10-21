using System;
using Biseth.Net.Couch.Http;
using Biseth.Net.Couch.Serialization;

namespace Biseth.Net.Couch.Threading
{
    public class RequestAsyncResult<TIn, TOut> : BasicAsyncResult
    {
        internal RequestAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            HttpClient = null;
            RequestData = null;
            ResponseData = null;
            Serializer = new NewtonsoftSerializer<TIn, TOut>();
        }

        public string Method { get; set; }
        public IHttpClient HttpClient { get; set; }
        public RequestData<TIn> RequestData { get; set; }
        public ResponseData<TOut> ResponseData { get; set; }
        public ISerializer<TIn, TOut> Serializer { get; set; }
    }
}