using System;
using Biseth.Net.Settee.Serialization;

namespace Biseth.Net.Settee.Http
{
    public interface IRequestClient
    {
        IAsyncResult BeginGet<TOut>(string path, AsyncCallback callback, object state);
        ResponseData<TOut> EndGet<TOut>(IAsyncResult ar);
        ResponseData<TOut> Get<TOut>(string path);

        IAsyncResult BeginHead<TOut>(string path, AsyncCallback callback, object state);
        ResponseData<TOut> EndHead<TOut>(IAsyncResult ar);
        ResponseData<TOut> Head<TOut>(string path);

        IAsyncResult BeginOptions<TOut>(string path, AsyncCallback callback, object state);
        ResponseData<TOut> EndOptions<TOut>(IAsyncResult ar);
        ResponseData<TOut> Options<TOut>(string path);

        IAsyncResult BeginDelete<TOut>(string path, AsyncCallback callback, object state);
        ResponseData<TOut> EndDelete<TOut>(IAsyncResult ar);
        ResponseData<TOut> Delete<TOut>(string path);

        IAsyncResult BeginPut<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);
        ResponseData<TOut> EndPut<TIn, TOut>(IAsyncResult ar);
        ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn> requestData);

        IAsyncResult BeginPost<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);
        ResponseData<TOut> EndPost<TIn, TOut>(IAsyncResult ar);
        ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData);
    }
}