using System;
using Biseth.Net.Settee.Serialization;

namespace Biseth.Net.Settee.Http
{
    public interface IRequestClient
    {
        IAsyncResult BeginGet<TOut>(string url, AsyncCallback callback, object state);
        ResponseData<TOut> EndGet<TOut>(IAsyncResult ar);
        ResponseData<TOut> Get<TOut>(string url);

        IAsyncResult BeginPut<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);
        ResponseData<TOut> EndPut<TOut>(IAsyncResult ar);
        ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn> requestData);

        IAsyncResult BeginHead<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);
        ResponseData<TOut> EndHead<TOut>(IAsyncResult ar);
        ResponseData<TOut> Head<TIn, TOut>(RequestData<TIn> requestData);

        IAsyncResult BeginPost<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);
        ResponseData<TOut> EndPost<TOut>(IAsyncResult ar);
        ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData);

        IAsyncResult BeginDelete<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);
        ResponseData<TOut> EndDelete<TOut>(IAsyncResult ar);
        ResponseData<TOut> Delete<TIn, TOut>(RequestData<TIn> requestData);

        IAsyncResult BeginOptions<TIn, TOut>(RequestData<TOut> requestData, AsyncCallback callback, object state);
        ResponseData<TOut> EndOptions<TOut>(IAsyncResult ar);
        ResponseData<TOut> Options<TIn, TOut>(RequestData<TIn> requestData);
    }
}