using System;
using Biseth.Net.Settee.Serialization;
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

        public IAsyncResult BeginGet<TOut>(string url, AsyncCallback callback, object state)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");

            var asyncResult = new RequestAsyncResult<TOut, TOut>(callback, state)
                {
                    RequestData = new RequestData<TOut>(url),
                    ResponseData = new ResponseData<TOut>(),
                    HttpClient = _httpClient,
                    Method = HttpMethod.Get,
                };

            asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginGet(asyncResult.RequestData.Url, RequestCallback<TOut, TOut>, asyncResult);

            return asyncResult;
        }

        public ResponseData<TOut> EndGet<TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar is RequestAsyncResult<TOut, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar as RequestAsyncResult<TOut, TOut>;
            return asyncResult.ResponseData;
        }

        public ResponseData<TOut> Get<TOut>(string url)
        {
            var asyncResult = BeginGet<TOut>(url, null, null);
            if (asyncResult != null && (asyncResult.IsCompleted || asyncResult.AsyncWaitHandle.WaitOne()))
            {
                return EndGet<TOut>(asyncResult);
            }
            return null;
        }

        public IAsyncResult BeginPut<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            //Func<T, string> serialze = obj => _serializer.Serialize(obj);
            //asyncResult.InternalAsyncResult = serialze.BeginInvoke(asyncResult.RequestData.RequestObject, SerializeCallback<T>, asyncResult);
            throw new NotImplementedException();
        }

        public ResponseData<TOut> EndPut<TOut>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn> requestData)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginHead<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> EndHead<TOut>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> Head<TIn, TOut>(RequestData<TIn> requestData)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginPost<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> EndPost<TOut>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDelete<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> EndDelete<TOut>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> Delete<TIn, TOut>(RequestData<TIn> requestData)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOptions<TIn, TOut>(RequestData<TOut> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> EndOptions<TOut>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData<TOut> Options<TIn, TOut>(RequestData<TIn> requestData)
        {
            throw new NotImplementedException();
        }

        private void RequestCallback<TIn, TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<TIn, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<TIn, TOut>;

            switch (asyncResult.Method)
            {
                case HttpMethod.Get:
                    var responseData = asyncResult.HttpClient.EndGet(ar);
                    if (responseData == null)
                    {
                        asyncResult.SetComplete();
                        return;
                    }
                    asyncResult.ResponseData.Data = responseData.Data;
                    asyncResult.ResponseData.ContentLength = responseData.ContentLength;
                    asyncResult.ResponseData.ContentType = responseData.ContentType;
                    asyncResult.ResponseData.StatusCode = responseData.StatusCode;
                    asyncResult.ResponseData.StatusDescription = responseData.StatusDescription;
                    asyncResult.InternalAsyncResult = asyncResult.Serializer.DeserializeFunc.BeginInvoke(responseData.Data, DeserializeCallback<TIn, TOut>, asyncResult);
                    break;
                default:
                    asyncResult.SetComplete();
                    break;
            }
        }

        private void SerializeCallback<TIn, TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<TIn, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<TIn, TOut>;
            switch (asyncResult.Method)
            {
                case HttpMethod.Put:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginPut(asyncResult.RequestData.Url, RequestCallback<TIn, TOut>, asyncResult);
                    break;
                case HttpMethod.Head:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginHead(asyncResult.RequestData.Url, RequestCallback<TIn, TOut>, asyncResult);
                    break;
                case HttpMethod.Post:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginPost(asyncResult.RequestData.Url, RequestCallback<TIn, TOut>, asyncResult);
                    break;
                case HttpMethod.Delete:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginDelete(asyncResult.RequestData.Url, RequestCallback<TIn, TOut>, asyncResult);
                    break;
                case HttpMethod.Options:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginOptions(asyncResult.RequestData.Url, RequestCallback<TIn, TOut>, asyncResult);
                    break;
                default:
                    asyncResult.SetComplete();
                    break;
            }
        }

        private void DeserializeCallback<TIn, TOut>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<TIn, TOut>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<TIn, TOut>;
            switch (asyncResult.Method)
            {
                case HttpMethod.Get:
                    asyncResult.ResponseData.DataDeserialized = asyncResult.Serializer.DeserializeFunc.EndInvoke(ar);
                    break;
            }
            asyncResult.SetComplete();
        }

    }
}