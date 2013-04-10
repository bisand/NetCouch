using System;

namespace Biseth.Net.Settee.Http
{
    public interface IHttpClient
    {
        IAsyncResult BeginGet(string path, AsyncCallback callback, object state);
        HttpResponseData EndGet(IAsyncResult asyncResult);
        HttpResponseData Get(string path);

        IAsyncResult BeginPut(string path, AsyncCallback callback, object state);
        string EndPut(IAsyncResult asyncResult);
        string Put(string path);

        IAsyncResult BeginHead(string path, AsyncCallback callback, object state);
        string EndHead(IAsyncResult asyncResult);
        string Head(string path);

        IAsyncResult BeginPost(string path, AsyncCallback callback, object state);
        string EndPost(IAsyncResult asyncResult);
        string Post(string path);

        IAsyncResult BeginDelete(string path, AsyncCallback callback, object state);
        string EndDelete(IAsyncResult asyncResult);
        string Delete(string path);

        IAsyncResult BeginOptions(string path, AsyncCallback callback, object state);
        string EndOptions(IAsyncResult asyncResult);
        string Options(string path);
    }
}