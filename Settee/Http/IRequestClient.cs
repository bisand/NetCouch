using System;
using Biseth.Net.Settee.Serialization;

namespace Biseth.Net.Settee.Http
{
    public interface IRequestClient
    {
        IAsyncResult BeginGet(string url, AsyncCallback callback, object state);
        ResponseData EndGet<T>(IAsyncResult ar);
        ResponseData Get<T>(string url, AsyncCallback callback, object state);

        IAsyncResult BeginPut<T>(RequestData<T> requestData, AsyncCallback callback, object state);
        ResponseData EndPut<T>(IAsyncResult ar);
        ResponseData Put<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);

        IAsyncResult BeginHead<T>(RequestData<T> requestData, AsyncCallback callback, object state);
        ResponseData EndHead<T>(IAsyncResult ar);
        ResponseData Head<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);

        IAsyncResult BeginPost<T>(RequestData<T> requestData, AsyncCallback callback, object state);
        ResponseData EndPost<T>(IAsyncResult ar);
        ResponseData Post<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);

        IAsyncResult BeginDelete<T>(RequestData<T> requestData, AsyncCallback callback, object state);
        ResponseData EndDelete<T>(IAsyncResult ar);
        ResponseData Delete<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);

        IAsyncResult BeginOptions<T>(RequestData<T> requestData, AsyncCallback callback, object state);
        ResponseData EndOptions<T>(IAsyncResult ar);
        ResponseData Options<TIn, TOut>(RequestData<TIn> requestData, AsyncCallback callback, object state);
    }
}