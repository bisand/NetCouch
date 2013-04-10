using System;
using System.Net;
using System.Text;
using Biseth.Net.Settee.Threading;

namespace Biseth.Net.Settee.Http
{
    public class HttpClient : IHttpClient
    {
        public string BaseUrl { get; private set; }

        public HttpClient(string baseUrl)
        {
            BaseUrl = baseUrl;
            BaseUri = new Uri(baseUrl);
        }

        public IAsyncResult BeginGet(string path, AsyncCallback callback, object state)
        {
            var builder = new UriBuilder(BaseUri) {Path = path};
            var asyncResult = new HttpAsyncResult(callback, state) {Request = (HttpWebRequest) WebRequest.Create(builder.Uri)};
            asyncResult.InternalAsyncResult = asyncResult.Request.BeginGetResponse(GetSersponseCallback, asyncResult);
            return asyncResult;
        }

        protected Uri BaseUri { get; private set; }

        public HttpResponseData EndGet(IAsyncResult ar)
        {
            if (ar == null || !(ar is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar as HttpAsyncResult;
            var responseData = new HttpResponseData
                {
                    Data = asyncResult.ResponseData.ToString(), 
                    ContentLength = asyncResult.Response.ContentLength, 
                    ContentType = asyncResult.Response.ContentType, 
                    StatusCode = asyncResult.Response.StatusCode, 
                    StatusDescription = asyncResult.Response.StatusDescription
                };
            return responseData;
        }

        public HttpResponseData Get(string path)
        {
            var asyncResult = BeginGet(path, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndGet(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginPut(string path, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public string EndPut(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public string Put(string path)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginHead(string path, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public string EndHead(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public string Head(string path)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginPost(string path, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public string EndPost(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public string Post(string path)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDelete(string path, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public string EndDelete(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public string Delete(string path)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOptions(string path, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public string EndOptions(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public string Options(string path)
        {
            throw new NotImplementedException();
        }

        private static void GetSersponseCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as HttpAsyncResult;
            asyncResult.Response = (HttpWebResponse) asyncResult.Request.EndGetResponse(ar);
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