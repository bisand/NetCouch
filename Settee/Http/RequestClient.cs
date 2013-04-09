using System;
using Biseth.Net.Settee.Serialization;
using Biseth.Net.Settee.Threading;

namespace Biseth.Net.Settee.Http
{
    public class RequestClient : IRequestClient
    {
        private readonly IHttpClient _httpClient;
        private readonly ISerializer _serializer;
        private string _url;

        public RequestClient(string url)
            : this(url, new HttpClient(), new NewtonsoftSerializer())
        {
        }

        public RequestClient(string url, IHttpClient httpClient, ISerializer serializer)
        {
            _url = url;
            _httpClient = httpClient;
            _serializer = serializer;
        }

        public IAsyncResult BeginGet(string url, AsyncCallback callback, object state)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");

            var asyncResult = new RequestAsyncResult<object>(callback, state)
                {
                    RequestData = new RequestData<object>(url),
                    Serializer = _serializer,
                    HttpClient = _httpClient,
                };
            asyncResult.Method = HttpMethod.Get;

            asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginGet(asyncResult.RequestData.Url, RequestCallback<object>, asyncResult);

            return asyncResult;
        }

        public ResponseData EndGet<T>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData Get<T>(string url, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginPut<T>(RequestData<T> requestData, AsyncCallback callback, object state)
        {
            //Func<T, string> serialze = obj => _serializer.Serialize(obj);
            //asyncResult.InternalAsyncResult = serialze.BeginInvoke(asyncResult.RequestData.RequestObject, SerializeCallback<T>, asyncResult);
            throw new NotImplementedException();
        }

        public ResponseData EndPut<T>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData Put<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginHead<T>(RequestData<T> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData EndHead<T>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData Head<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginPost<T>(RequestData<T> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData EndPost<T>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData Post<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDelete<T>(RequestData<T> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData EndDelete<T>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData Delete<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOptions<T>(RequestData<T> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public ResponseData EndOptions<T>(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public ResponseData Options<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        private void RequestCallback<T>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<T>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<T>;

            switch (asyncResult.Method)
            {
                case HttpMethod.Get:
                    asyncResult.ResponseData.Data = asyncResult.HttpClient.EndGet(ar);
                    Func<string, T> deserialze = text => _serializer.Deserialize<T>(text);
                    asyncResult.InternalAsyncResult = deserialze.BeginInvoke(asyncResult.ResponseData.Data, DeserializeCallback<T>, asyncResult);
                    break;
                default:
                    asyncResult.SetComplete();
                    break;
            }
        }

        private void SerializeCallback<T>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<T>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<T>;
            switch (asyncResult.Method)
            {
                case HttpMethod.Put:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginPut(asyncResult.RequestData.Url, RequestCallback<T>, asyncResult);
                    break;
                case HttpMethod.Head:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginHead(asyncResult.RequestData.Url, RequestCallback<T>, asyncResult);
                    break;
                case HttpMethod.Post:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginPost(asyncResult.RequestData.Url, RequestCallback<T>, asyncResult);
                    break;
                case HttpMethod.Delete:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginDelete(asyncResult.RequestData.Url, RequestCallback<T>, asyncResult);
                    break;
                case HttpMethod.Options:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginOptions(asyncResult.RequestData.Url, RequestCallback<T>, asyncResult);
                    break;
                default:
                    asyncResult.SetComplete();
                    break;
            }
        }

        private void DeserializeCallback<T>(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is RequestAsyncResult<T>))
                throw new NullReferenceException("Async result is null or async state is not of the expected type.");

            var asyncResult = ar.AsyncState as RequestAsyncResult<T>;
            switch (asyncResult.Method)
            {
                case HttpMethod.Get:
                    asyncResult.InternalAsyncResult = asyncResult.HttpClient.BeginPut(asyncResult.RequestData.Url, RequestCallback<T>, asyncResult);
                    break;
                default:
                    asyncResult.SetComplete();
                    break;
            }
        }

    }
}