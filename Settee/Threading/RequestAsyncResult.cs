using System;
using System.IO;
using System.Net;
using System.Text;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Serialization;

namespace Biseth.Net.Settee.Threading
{
    public class RequestAsyncResult<T> : BasicAsyncResult
    {
        internal RequestAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            HttpClient = null;
            RequestData = null;
            ResponseData = null;
        }

        public string Method { get; set; }
        public IHttpClient HttpClient { get; set; }
        public RequestData<T> RequestData { get; set; }
        public ResponseData ResponseData { get; set; }
        public ISerializer Serializer { get; set; }
    }
}