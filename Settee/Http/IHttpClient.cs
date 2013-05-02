using System;

namespace Biseth.Net.Settee.Http
{
    public interface IHttpClient : IDisposable
    {
        IAsyncResult BeginGet(HttpRequestData requestData, AsyncCallback callback, object state);
        HttpResponseData EndGet(IAsyncResult ar);
        HttpResponseData Get(HttpRequestData requestData);

        IAsyncResult BeginHead(HttpRequestData requestData, AsyncCallback callback, object state);
        HttpResponseData EndHead(IAsyncResult ar);
        HttpResponseData Head(HttpRequestData requestData);

        IAsyncResult BeginDelete(HttpRequestData requestData, AsyncCallback callback, object state);
        HttpResponseData EndDelete(IAsyncResult ar);
        HttpResponseData Delete(HttpRequestData requestData);

        IAsyncResult BeginOptions(HttpRequestData requestData, AsyncCallback callback, object state);
        HttpResponseData EndOptions(IAsyncResult ar);
        HttpResponseData Options(HttpRequestData requestData);

        IAsyncResult BeginPut(HttpRequestData requestData, AsyncCallback callback, object state);
        HttpResponseData EndPut(IAsyncResult ar);
        HttpResponseData Put(HttpRequestData requestData);

        IAsyncResult BeginPost(HttpRequestData requestData, AsyncCallback callback, object state);
        HttpResponseData EndPost(IAsyncResult ar);
        HttpResponseData Post(HttpRequestData requestData);
    }
}