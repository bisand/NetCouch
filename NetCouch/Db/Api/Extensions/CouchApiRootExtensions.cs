using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using NetCouch.Db.Api.Elements;
using NetCouch.Http;
using NetCouch.Models.Couch.Doc;

namespace NetCouch.Db.Api.Extensions
{
    public static class CouchApiRootExtensions
    {
        public static CouchApiRootCommand ActiveTasks(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_active_tasks/"
                };
            return result;
        }

        public static CouchApiRootCommand AllDbs(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_all_dbs/"
                };
            return result;
        }

        public static CouchApiRootCommand Log(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_log/"
                };
            return result;
        }

        public static CouchApiRootCommand Replicate(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_replicate/"
                };
            return result;
        }

        public static CouchApiRootCommand Restart(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_restart/"
                };
            return result;
        }

        public static CouchApiRootCommand Stats(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_stats/"
                };
            return result;
        }

        public static CouchApiRootCommand Utils(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_utils/"
                };
            return result;
        }

        public static CouchApiRootCommand UuIds(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_uuids/"
                };
            return result;
        }

        public static CouchApiRootCommand Favicon(this CouchApiRoot element)
        {
            var result = new CouchApiRootCommand(element._requestClient)
                {
                    PathElement = element.PathElement + "_favicon.ico/"
                };
            return result;
        }

        public static ResponseData<object> Head(this CouchApiRoot element)
        {
            var responseData = element._requestClient.Head<object>(element.PathElement);
            return responseData;
        }

        public static ResponseData<T> Get<T>(this CouchApiRoot element)
        {
            var responseData = element._requestClient.Get<T>(element.PathElement);
            return responseData;
        }

        public static ResponseData<TOut> Put<TIn, TOut>(this CouchApiRoot element, TIn obj = default(TIn), string revision = null)
        {
            var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
            if (!string.IsNullOrWhiteSpace(revision))
                requestData.Headers = new WebHeaderCollection {{"If-Match", revision}};
            var responseData = element._requestClient.Put<TIn, TOut>(requestData);
            return responseData;
        }

        public static ResponseData<object> Put(this CouchApiRoot element, dynamic obj = default(dynamic))
        {
            var requestData = new RequestData<dynamic>(element.PathElement, obj, "application/json");
            var responseData = element._requestClient.Put<dynamic, object>(requestData);
            return responseData;
        }

        public static ResponseData<TOut> Post<TIn, TOut>(this CouchApiRoot element, TIn obj = default(TIn))
        {
            var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
            var responseData = element._requestClient.Post<TIn, TOut>(requestData);
            return responseData;
        }
    }
}