using System;
using System.Net;
using System.Text;
using Biseth.Net.Settee.Threading;

namespace Biseth.Net.Settee.Http
{
    public class HttpClient : IHttpClient
    {
        public IAsyncResult BeginGet(string url, AsyncCallback callback, object state)
        {
            var asyncResult = new HttpAsyncResult(callback, state) {Request = WebRequest.Create(url)};
            asyncResult.InternalAsyncResult = asyncResult.Request.BeginGetResponse(GetSersponseCallback, asyncResult);
            return asyncResult;
        }

        public string EndGet(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as HttpAsyncResult;
            return asyncResult.ResponseData.ToString();
        }

        public string Get(string url)
        {
            var asyncResult = BeginGet(url, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndGet(asyncResult);
            }
            return null;
        }

        private static void GetSersponseCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as HttpAsyncResult;
            asyncResult.Response = asyncResult.Request.EndGetResponse(ar);
            asyncResult.ResponseStream = asyncResult.Response.GetResponseStream();
            if (asyncResult.ResponseStream != null)
                asyncResult.InternalAsyncResult = asyncResult.ResponseStream.BeginRead(asyncResult.BufferRead, 0, asyncResult.BufferReadSize, BufferReadCallback, asyncResult);
            else
                asyncResult.SetComplete();
        }

        private static void BufferReadCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as HttpAsyncResult;
            var read = asyncResult.ResponseStream.EndRead(ar);
            if (read > 0)
            {
                asyncResult.ResponseData.Append(Encoding.UTF8.GetString(asyncResult.BufferRead, 0, read));
                asyncResult.InternalAsyncResult = asyncResult.ResponseStream.BeginRead(asyncResult.BufferRead, 0, asyncResult.BufferReadSize, BufferReadCallback, asyncResult);
            }
            else
            {
                asyncResult.ResponseStream.Close();
                asyncResult.Response.Close();
                asyncResult.SetComplete();
            }
        }
    }
}