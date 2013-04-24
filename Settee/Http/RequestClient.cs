using System;
using System.Net;
using Biseth.Net.Settee.Threading;

namespace Biseth.Net.Settee.Http
{
    public class RequestClient : IRequestClient
    {
        private readonly IHttpClient _httpClient;
        private string _url;

        public RequestClient(string url)
            : this(url, new HttpClient(url))
        {
        }

        public RequestClient(string url, IHttpClient httpClient)
        {
            _url = url;
            _httpClient = httpClient;
        }

        public IAsyncResult BeginGet<TOut>(string path, AsyncCallback callback, object state)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var asyncResult = new RequestAsyncResult<TOut, TOut>(callback, state)
                {
                    RequestData = new RequestData<TOut>(path),
                    ResponseData = new ResponseData<TOut>(),
                    HttpClient = _httpClient,
                    Method = HttpMethod.Get,
                };
            var httpRequestData = new HttpRequestData(path);
            httpRequestData.Method = HttpMethod.Get;
            asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginGet(httpRequestData, RequestCallback<TOut, TOut>, asyncResult);

            return asyncResult;
        }

        public ResponseData<TOut> EndGet<TOut>(IAsyncResult ar)
        {
            return GetResponseData<TOut>(ar);
        }

        public ResponseData<TOut> Get<TOut>(string path)
        {
            var asyncResult = BeginGet<TOut>(path, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndGet<TOut>(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginHead<TOut>(string path, AsyncCallback callback, object state)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var asyncResult = new RequestAsyncResult<TOut, TOut>(callback, state)
                {
                    RequestData = new RequestData<TOut>(path),
                    ResponseData = new ResponseData<TOut>(),
                    HttpClient = _httpClient,
                    Method = HttpMethod.Head,
                };

            var httpRequestData = new HttpRequestData(path);
            httpRequestData.Method = HttpMethod.Head;
            asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginHead(httpRequestData, RequestCallback<TOut, TOut>, asyncResult);

            return asyncResult;
        }

        public ResponseData<TOut> EndHead<TOut>(IAsyncResult ar)
        {
            return GetResponseData<TOut>(ar);
        }

        public ResponseData<TOut> Head<TOut>(string path)
        {
            var asyncResult = BeginHead<TOut>(path, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndHead<TOut>(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginOptions<TOut>(string path, AsyncCallback callback, object state)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var asyncResult = new RequestAsyncResult<TOut, TOut>(callback, state)
                {
                    RequestData = new RequestData<TOut>(path),
                    ResponseData = new ResponseData<TOut>(),
                    HttpClient = _httpClient,
                    Method = HttpMethod.Options,
                };

            var httpRequestData = new HttpRequestData(path);
            httpRequestData.Method = HttpMethod.Options;
            asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginOptions(httpRequestData, RequestCallback<TOut, TOut>, asyncResult);

            return asyncResult;
        }

        public ResponseData<TOut> EndOptions<TOut>(IAsyncResult ar)
        {
            return GetResponseData<TOut>(ar);
        }

        public ResponseData<TOut> Options<TOut>(string path)
        {
            var asyncResult = BeginOptions<TOut>(path, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndOptions<TOut>(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginDelete<TOut>(string path, AsyncCallback callback, object state)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var asyncResult = new RequestAsyncResult<TOut, TOut>(callback, state)
                {
                    RequestData = new RequestData<TOut>(path),
                    ResponseData = new ResponseData<TOut>(),
                    HttpClient = _httpClient,
                    Method = HttpMethod.Delete,
                };

            var httpRequestData = new HttpRequestData(path);
            httpRequestData.Method = HttpMethod.Delete;
            asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginDelete(httpRequestData, RequestCallback<TOut, TOut>, asyncResult);

            return asyncResult;
        }

        public ResponseData<TOut> EndDelete<TOut>(IAsyncResult ar)
        {
            return GetResponseData<TOut>(ar);
        }

        public ResponseData<TOut> Delete<TOut>(string path)
        {
            var asyncResult = BeginDelete<TOut>(path, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndDelete<TOut>(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginPut<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            if (requestData == null)
                throw new ArgumentNullException("requestData");

            var url = requestData.Url;
            var asyncResult = new RequestAsyncResult<TIn, TOut>(callback, state)
                {
                    RequestData = requestData,
                    ResponseData = new ResponseData<TOut>(),
                    HttpClient = _httpClient,
                    Method = HttpMethod.Put,
                };

            asyncResult.InternalAsyncResult = asyncResult.Serializer.SerializeFunc.BeginInvoke(requestData.RequestObject, SerializeCallback<TIn, TOut>, asyncResult);

            return asyncResult;
        }

        public ResponseData<TOut> EndPut<TIn, TOut>(IAsyncResult ar)
        {
            return GetResponseData<TIn, TOut>(ar);
        }

        public ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn> requestData)
        {
            var asyncResult = BeginPut<TIn, TOut>(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndPut<TIn, TOut>(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginPost<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            if (requestData == null)
                throw new ArgumentNullException("requestData");

            var url = requestData.Url;
            var asyncResult = new RequestAsyncResult<TIn, TOut>(callback, state)
                {
                    RequestData = requestData,
                    ResponseData = new ResponseData<TOut>(),
                    HttpClient = _httpClient,
                    Method = HttpMethod.Post,
                };

            asyncResult.InternalAsyncResult = asyncResult.Serializer.SerializeFunc.BeginInvoke(requestData.RequestObject, SerializeCallback<TIn, TOut>, asyncResult);

            return asyncResult;
        }

        public ResponseData<TOut> EndPost<TIn, TOut>(IAsyncResult ar)
        {
            return GetResponseData<TIn, TOut>(ar);
        }

        public ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData)
        {
            var asyncResult = BeginPost<TIn, TOut>(requestData, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndPost<TIn, TOut>(asyncResult);
            }
            return null;
        }

        private static ResponseData<TOut> GetResponseData<TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar is RequestAsyncResult<TOut, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar as RequestAsyncResult<TOut, TOut>;
            return asyncResult.ResponseData;
        }

        private static ResponseData<TOut> GetResponseData<TIn, TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar is RequestAsyncResult<TIn, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar as RequestAsyncResult<TIn, TOut>;
            return asyncResult.ResponseData;
        }

        private static void SerializeCallback<TIn, TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<TIn, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<TIn, TOut>;
            var data = asyncResult.Serializer.SerializeFunc.EndInvoke(ar);

            switch (asyncResult.Method)
            {
                case HttpMethod.Put:
                    var dataPut = new HttpRequestData(asyncResult.RequestData.Url, "", "application/json", HttpMethod.Put, data);
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginPut(dataPut, RequestCallback<TIn, TOut>, asyncResult);
                    break;
                case HttpMethod.Post:
                    var dataPost = new HttpRequestData(asyncResult.RequestData.Url, "", "application/json", HttpMethod.Post, data);
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginPost(dataPost, RequestCallback<TIn, TOut>, asyncResult);
                    break;
                default:
                    asyncResult.SetComplete();
                    break;
            }
        }

        private static void RequestCallback<TIn, TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<TIn, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<TIn, TOut>;

            HttpResponseData responseData = null;
            switch (asyncResult.Method)
            {
                case HttpMethod.Get:
                    responseData = asyncResult.HttpClient.EndGet(ar);
                    break;
                case HttpMethod.Head:
                    responseData = asyncResult.HttpClient.EndHead(ar);
                    break;
                case HttpMethod.Options:
                    responseData = asyncResult.HttpClient.EndOptions(ar);
                    break;
                case HttpMethod.Delete:
                    responseData = asyncResult.HttpClient.EndDelete(ar);
                    break;
                case HttpMethod.Put:
                    responseData = asyncResult.HttpClient.EndPut(ar);
                    break;
                case HttpMethod.Post:
                    responseData = asyncResult.HttpClient.EndPost(ar);
                    break;
            }
            if (responseData == null)
            {
                asyncResult.ResponseData.StatusCode = HttpStatusCode.InternalServerError;
                asyncResult.ResponseData.StatusDescription = "An error occurred!";
                asyncResult.SetComplete();
                return;
            }
            asyncResult.ResponseData.Data = responseData.Data;
            asyncResult.ResponseData.ContentLength = responseData.ContentLength;
            asyncResult.ResponseData.ContentType = responseData.ContentType;
            asyncResult.ResponseData.StatusCode = responseData.StatusCode;
            asyncResult.ResponseData.StatusDescription = responseData.StatusDescription;
            if (!string.IsNullOrWhiteSpace(responseData.Data))
            {
                asyncResult.InternalAsyncResult = asyncResult.Serializer.DeserializeFunc.BeginInvoke(responseData.Data, DeserializeCallback<TIn, TOut>, asyncResult);
                return;
            }

            asyncResult.SetComplete();
        }

        private static void DeserializeCallback<TIn, TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<TIn, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<TIn, TOut>;
            asyncResult.ResponseData.DataDeserialized = asyncResult.Serializer.DeserializeFunc.EndInvoke(ar);
            asyncResult.SetComplete();
        }
    }
}