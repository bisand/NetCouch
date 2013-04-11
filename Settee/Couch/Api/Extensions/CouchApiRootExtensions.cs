using Biseth.Net.Settee.Couch.Api.Elements;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Models.Couch.Database;

namespace Biseth.Net.Settee.Couch.Api.Extensions
{
    public static class CouchApiRootExtensions
    {
        public static ResponseData<T> Get<T>(this CouchApiRoot element)
        {
            var responseData = element.RequestClient.Get<T>(element.PathElement);
            return responseData;
        }

        public static ResponseData<TOut> Put<TIn, TOut>(this CouchApiRoot element, TIn obj = default(TIn))
        {
            var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
            var responseData = element.RequestClient.Put<TIn, TOut>(requestData);
            return responseData;
        }

        public static ResponseData<TOut> Post<TIn, TOut>(this CouchApiRoot element, TIn obj = default(TIn))
        {
            var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
            var responseData = element.RequestClient.Post<TIn, TOut>(requestData);
            return responseData;
        }
    }
}