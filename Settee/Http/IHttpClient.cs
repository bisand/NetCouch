using System;

namespace Biseth.Net.Settee.Http
{
    public interface IHttpClient
    {
        IAsyncResult BeginGet(string url, AsyncCallback callback, object state);
        string EndGet(IAsyncResult asyncResult);
        string Get(string url);

        IAsyncResult BeginPut(string url, AsyncCallback callback, object state);
        string EndPut(IAsyncResult asyncResult);
        string Put(string url);

        IAsyncResult BeginHead(string url, AsyncCallback callback, object state);
        string EndHead(IAsyncResult asyncResult);
        string Head(string url);

        IAsyncResult BeginPost(string url, AsyncCallback callback, object state);
        string EndPost(IAsyncResult asyncResult);
        string Post(string url);

        IAsyncResult BeginDelete(string url, AsyncCallback callback, object state);
        string EndDelete(IAsyncResult asyncResult);
        string Delete(string url);

        IAsyncResult BeginOptions(string url, AsyncCallback callback, object state);
        string EndOptions(IAsyncResult asyncResult);
        string Options(string url);
    }
}