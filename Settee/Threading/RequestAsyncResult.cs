using System;
using System.IO;
using System.Net;
using System.Text;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Serialization;

namespace Biseth.Net.Settee.Threading
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