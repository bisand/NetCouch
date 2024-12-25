using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetCouch.Extensions;

namespace NetCouch.Http
{
    public class HttpClient
    {
        private bool _disposed;

        public HttpClient(string baseUrl)
        {
            BaseUrl = baseUrl;
            BaseUri = new Uri(baseUrl);
        }

        public string BaseUrl { get; private set; }

        protected Uri BaseUri { get; private set; }

        public async Task<HttpResponseData> GetAsync(HttpRequestData requestData)
        {
            return await StartProcessingAsync(requestData);
        }

        public HttpResponseData Get(HttpRequestData requestData)
        {
            return GetAsync(requestData).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseData> PutAsync(HttpRequestData requestData)
        {
            return await StartProcessingAsync(requestData);
        }

        public HttpResponseData Put(HttpRequestData requestData)
        {
            return PutAsync(requestData).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseData> HeadAsync(HttpRequestData requestData)
        {
            return await StartProcessingAsync(requestData);
        }

        public HttpResponseData Head(HttpRequestData requestData)
        {
            return HeadAsync(requestData).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseData> PostAsync(HttpRequestData requestData)
        {
            return await StartProcessingAsync(requestData);
        }

        public HttpResponseData Post(HttpRequestData requestData)
        {
            return PostAsync(requestData).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseData> DeleteAsync(HttpRequestData requestData)
        {
            return await StartProcessingAsync(requestData);
        }

        public HttpResponseData Delete(HttpRequestData requestData)
        {
            return DeleteAsync(requestData).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseData> OptionsAsync(HttpRequestData requestData)
        {
            return await StartProcessingAsync(requestData);
        }

        public HttpResponseData Options(HttpRequestData requestData)
        {
            return OptionsAsync(requestData).GetAwaiter().GetResult();
        }

        private async Task<HttpResponseData> StartProcessingAsync(HttpRequestData requestData)
        {
            Uri uri = BaseUri.Append(requestData.Path);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            if (requestData.Headers != null)
                request.Headers = requestData.Headers;

            request.Method = requestData.Method;
            request.Referer = BaseUri.ToString();
            if (requestData.ContentType != null)
                request.ContentType = requestData.ContentType;
            if (requestData.Data != null)
            {
                var buffer = Encoding.UTF8.GetBytes(requestData.Data.ToString());
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }

            try
            {
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    return await GetHttpResponseDataAsync(response);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var response = (HttpWebResponse)ex.Response)
                    {
                        return await GetHttpResponseDataAsync(response);
                    }
                }
                throw;
            }
        }

        private async Task<HttpResponseData> GetHttpResponseDataAsync(HttpWebResponse response)
        {
            var responseData = new HttpResponseData
            {
                ContentLength = response.ContentLength,
                ContentType = response.ContentType,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
            };

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseData.Data = await reader.ReadToEndAsync();
                    }
                }
            }

            return responseData;
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