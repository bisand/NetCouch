using System;
using System.Net;
using System.Text;
using Biseth.Net.Couch.Threading;

namespace Biseth.Net.Couch.Http
{
    public class HttpClient : IHttpClient
    {
        private bool _disposed;

        public HttpClient(string baseUrl)
        {
            BaseUrl = baseUrl;
            BaseUri = new Uri(baseUrl);
        }

        public string BaseUrl { get; private set; }

        protected Uri BaseUri { get; private set; }

        public IAsyncResult BeginGet(HttpRequestData requestData, AsyncCallback callback, object state)
        {
            return StartProcessing(requestData, callback, state);
        }

        public HttpResponseData EndGet(IAsyncResult ar)
        {
            return GetHttpResponseData(ar);
        }

        public HttpResponseData Get(HttpRequestData requestData)
        {
            var asyncResult = BeginGet(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndGet(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginPut(HttpRequestData requestData, AsyncCallback callback, object state)
        {
            return StartProcessing(requestData, callback, state);
        }

        public HttpResponseData EndPut(IAsyncResult ar)
        {
            return GetHttpResponseData(ar);
        }

        public HttpResponseData Put(HttpRequestData requestData)
        {
            var asyncResult = BeginPut(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndPut(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginHead(HttpRequestData requestData, AsyncCallback callback, object state)
        {
            return StartProcessing(requestData, callback, state);
        }

        public HttpResponseData EndHead(IAsyncResult ar)
        {
            return GetHttpResponseData(ar);
        }

        public HttpResponseData Head(HttpRequestData requestData)
        {
            var asyncResult = BeginHead(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndHead(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginPost(HttpRequestData requestData, AsyncCallback callback, object state)
        {
            return StartProcessing(requestData, callback, state);
        }

        public HttpResponseData EndPost(IAsyncResult ar)
        {
            return GetHttpResponseData(ar);
        }

        public HttpResponseData Post(HttpRequestData requestData)
        {
            var asyncResult = BeginPost(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndPost(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginDelete(HttpRequestData requestData, AsyncCallback callback, object state)
        {
            return StartProcessing(requestData, callback, state);
        }

        public HttpResponseData EndDelete(IAsyncResult ar)
        {
            return GetHttpResponseData(ar);
        }

        public HttpResponseData Delete(HttpRequestData requestData)
        {
            var asyncResult = BeginDelete(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndDelete(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginOptions(HttpRequestData requestData, AsyncCallback callback, object state)
        {
            return StartProcessing(requestData, callback, state);
        }

        public HttpResponseData EndOptions(IAsyncResult ar)
        {
            return GetHttpResponseData(ar);
        }

        public HttpResponseData Options(HttpRequestData requestData)
        {
            var asyncResult = BeginOptions(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndOptions(asyncResult);
            }
            return null;
        }

        private IAsyncResult StartProcessing(HttpRequestData requestData, AsyncCallback callback, object state)
        {
            var uri = new Uri(BaseUri, requestData.Path);
            var asyncResult = new HttpAsyncResult(callback, state)
                {
                    Request = (HttpWebRequest) WebRequest.Create(uri)
                };
            asyncResult.Request.Method = requestData.Method;
            asyncResult.Request.Referer = BaseUri.ToString();
            if (requestData.ContentType != null)
                asyncResult.Request.ContentType = requestData.ContentType;
            if (requestData.Data != null)
                asyncResult.RequestData.Append(requestData.Data);
            switch (requestData.Method)
            {
                case HttpMethod.Get:
                case HttpMethod.Head:
                case HttpMethod.Delete:
                case HttpMethod.Options:
                    asyncResult.InternalAsyncResult = asyncResult.Request.BeginGetResponse(GetResponseCallback, asyncResult);
                    return asyncResult;
                case HttpMethod.Put:
                case HttpMethod.Post:
                    asyncResult.InternalAsyncResult = asyncResult.Request.BeginGetRequestStream(GetRequestStreamCallback, asyncResult);
                    return asyncResult;
            }
            throw new ArgumentException(string.Format("Invalid method '{0}'", requestData.Method), "method");
        }

        private static HttpResponseData GetHttpResponseData(IAsyncResult ar)
        {
            if (ar == null || !(ar is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar as HttpAsyncResult;
            var responseData = new HttpResponseData
                {
                    Data = asyncResult.ResponseData.ToString(),
                    ContentLength = asyncResult.Response != null ? asyncResult.Response.ContentLength : 0,
                    ContentType = asyncResult.Response != null ? asyncResult.Response.ContentType : "",
                    StatusCode = asyncResult.Response != null ? asyncResult.Response.StatusCode : HttpStatusCode.InternalServerError,
                    StatusDescription = asyncResult.Response != null ? asyncResult.Response.StatusDescription : "An error occurred!",
                };
            return responseData;
        }

        private static void GetRequestStreamCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as HttpAsyncResult;
            try
            {
                asyncResult.RequestStream = asyncResult.Request.EndGetRequestStream(ar);
            }
            catch (WebException ex)
            {
                asyncResult.Response = (HttpWebResponse) ex.Response;
                asyncResult.ResponseStream = asyncResult.Response.GetResponseStream();
                if (asyncResult.ResponseStream != null)
                    asyncResult.InternalAsyncResult = asyncResult.ResponseStream.BeginRead(asyncResult.BufferRead, 0, asyncResult.BufferReadSize, BufferReadCallback, asyncResult);
                else
                    asyncResult.SetComplete();
                return;
            }
            catch (Exception ex)
            {
                asyncResult.Exception = ex;
                asyncResult.SetComplete();
                return;
            }
            var buffer = Encoding.UTF8.GetBytes(asyncResult.RequestData.ToString());
            asyncResult.InternalAsyncResult = asyncResult.RequestStream.BeginWrite(buffer, 0, buffer.Length, WriteRequestStreamCallback, asyncResult);
        }

        private static void WriteRequestStreamCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as HttpAsyncResult;
            try
            {
                asyncResult.RequestStream.EndWrite(ar);
            }
            catch (Exception ex)
            {
                asyncResult.Exception = ex;
                asyncResult.SetComplete();
                return;
            }
            asyncResult.InternalAsyncResult = asyncResult.Request.BeginGetResponse(GetResponseCallback, asyncResult);
        }

        private static void GetResponseCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is HttpAsyncResult))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as HttpAsyncResult;
            try
            {
                asyncResult.Response = (HttpWebResponse) asyncResult.Request.EndGetResponse(ar);
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    asyncResult.SetComplete();
                    return;
                }
                asyncResult.Response = (HttpWebResponse) ex.Response;
            }
            catch (Exception ex)
            {
                asyncResult.Exception = ex;
                asyncResult.SetComplete();
                return;
            }
            asyncResult.ResponseStream = asyncResult.Response.GetResponseStream();
            if (asyncResult.ResponseStream != null && asyncResult.ResponseStream.CanRead)
            {
                try
                {
                    asyncResult.InternalAsyncResult = asyncResult.ResponseStream.BeginRead(asyncResult.BufferRead, 0,
                                                                                           asyncResult.BufferReadSize,
                                                                                           BufferReadCallback, asyncResult);
                }
                catch (Exception ex)
                {
                    asyncResult.Exception = ex;
                    asyncResult.ResponseStream.Close();
                    asyncResult.Response.Close();
                    asyncResult.SetComplete();
                }
            }
            else
            {
                asyncResult.SetComplete();
            }
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

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
            }
            _disposed = true;
        }

        ~HttpClient()
        {
            Dispose(false);
        }

        #endregion
    }
}