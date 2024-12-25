using System;
using System.Net;
using System.Threading.Tasks;
using NetCouch.Threading;

namespace NetCouch.Http
{
    public class RequestClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;
        private string _url;

        public RequestClient(string url)
        {
            _url = url;
            _httpClient = new HttpClient(url);
        }

        public RequestClient(string url, HttpClient httpClient)
        {
            _url = url;
            _httpClient = httpClient;
        }

        public async Task<ResponseData<TOut>> GetAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var requestData = new RequestData<TOut>(path);
            var httpRequestData = new HttpRequestData(path) { Method = HttpMethod.Get };
            var response = await _httpClient.GetAsync(httpRequestData);
            return new ResponseData<TOut> { Data = response.Data, ContentLength = response.ContentLength, ContentType = response.ContentType, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
        }

        public ResponseData<TOut> Get<TOut>(string path)
        {
            return GetAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> HeadAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var requestData = new RequestData<TOut>(path);
            var httpRequestData = new HttpRequestData(path) { Method = HttpMethod.Head };
            var response = await _httpClient.HeadAsync(httpRequestData);
            return new ResponseData<TOut> { Data = response.Data, ContentLength = response.ContentLength, ContentType = response.ContentType, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
        }

        public ResponseData<TOut> Head<TOut>(string path)
        {
            return HeadAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> OptionsAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var requestData = new RequestData<TOut>(path);
            var httpRequestData = new HttpRequestData(path) { Method = HttpMethod.Options };
            var response = await _httpClient.OptionsAsync(httpRequestData);
            return new ResponseData<TOut> { Data = response.Data, ContentLength = response.ContentLength, ContentType = response.ContentType, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
        }

        public ResponseData<TOut> Options<TOut>(string path)
        {
            return OptionsAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> DeleteAsync<TOut>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            var requestData = new RequestData<TOut>(path);
            var httpRequestData = new HttpRequestData(path) { Method = HttpMethod.Delete };
            var response = await _httpClient.DeleteAsync(httpRequestData);
            return new ResponseData<TOut> { Data = response.Data, ContentLength = response.ContentLength, ContentType = response.ContentType, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
        }

        public ResponseData<TOut> Delete<TOut>(string path)
        {
            return DeleteAsync<TOut>(path).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> PutAsync<TIn, TOut>(RequestData<TIn> requestData)
        {
            if (requestData == null)
                throw new ArgumentNullException("requestData");

            var httpRequestData = new HttpRequestData(requestData.Url) { Method = HttpMethod.Put, Data = requestData.RequestObject.ToString(), ContentType = "application/json", Headers = requestData.Headers };
            var response = await _httpClient.PutAsync(httpRequestData);
            return new ResponseData<TOut> { Data = response.Data, ContentLength = response.ContentLength, ContentType = response.ContentType, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
        }

        public ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn> requestData)
        {
            return PutAsync<TIn, TOut>(requestData).GetAwaiter().GetResult();
        }

        public async Task<ResponseData<TOut>> PostAsync<TIn, TOut>(RequestData<TIn> requestData)
        {
            if (requestData == null)
                throw new ArgumentNullException("requestData");

            var httpRequestData = new HttpRequestData(requestData.Url) { Method = HttpMethod.Post, Data = requestData.RequestObject.ToString(), ContentType = "application/json", Headers = requestData.Headers };
            var response = await _httpClient.PostAsync(httpRequestData);
            return new ResponseData<TOut> { Data = response.Data, ContentLength = response.ContentLength, ContentType = response.ContentType, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
        }

        public ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData)
        {
            return PostAsync<TIn, TOut>(requestData).GetAwaiter().GetResult();
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

        ~RequestClient()
        {
            Dispose(false);
        }

        #endregion
    }
}