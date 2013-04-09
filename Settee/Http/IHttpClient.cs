using System;

namespace Biseth.Net.Settee.Http
{
    public interface IHttpClient
    {
        IAsyncResult BeginGet(string url, AsyncCallback callback, object state);
        string EndGet(IAsyncResult asyncResult);
    }
}