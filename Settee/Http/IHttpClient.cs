using System;
using System.Web;

namespace Settee.Http
{
    public interface IHttpClient
    {
        IAsyncResult BeginGet(string url, AsyncCallback callback, object state);
        string EndGet(IAsyncResult asyncResult);
    }
}