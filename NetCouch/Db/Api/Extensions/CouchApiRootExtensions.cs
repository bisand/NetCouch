using NetCouch.Db.Api.Elements;
using NetCouch.Http;
using NetCouch.Models.Couch.Database;

namespace NetCouch.Db.Api.Extensions;

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
        var responseData = (element._requestClient.Get<T>(element.PathElement)) ?? new ResponseData<T>();
        return responseData;
    }

    public static ResponseData<TOut> Put<TIn, TOut>(this CouchApiRoot element, TIn? obj = default, string? revision = null)
    {
        var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
        if (!string.IsNullOrWhiteSpace(revision))
        {
            // Ensure the revision string is enclosed in double quotes
            if (!revision.StartsWith('\"') && !revision.EndsWith('\"'))
            {
                revision = $"\"{revision}\"";
            }
            requestData?.Headers?.Add("If-Match", revision);
        }
        var responseData = element._requestClient.Put<TIn, TOut>(requestData);
        return responseData;
    }

    public static ResponseData<CouchDbResult> Put<TIn>(this CouchApiRoot element, TIn? obj = default, string? revision = null)
    {
        var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
        if (!string.IsNullOrWhiteSpace(revision))
        {
            // Ensure the revision string is enclosed in double quotes
            if (!revision.StartsWith('\"') && !revision.EndsWith('\"'))
            {
                revision = $"\"{revision}\"";
            }
            requestData?.Headers?.Add("If-Match", revision);
        }
        var responseData = element._requestClient.Put<TIn, CouchDbResult>(requestData);
        return responseData;
    }

    public static ResponseData<CouchDbResult> Put(this CouchApiRoot element, dynamic? obj = default)
    {
        var requestData = new RequestData<dynamic>(element.PathElement, obj, "application/json");
        var responseData = element._requestClient.Put<dynamic, CouchDbResult>(requestData);
        return responseData;
    }

    public static ResponseData<TOut> Post<TIn, TOut>(this CouchApiRoot element, TIn? obj = default)
    {
        var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
        var responseData = element._requestClient.Post<TIn, TOut>(requestData);
        return responseData;
    }

    public static ResponseData<CouchDbResult> Post<TIn>(this CouchApiRoot element, TIn? obj = default)
    {
        var requestData = new RequestData<TIn>(element.PathElement, obj, "application/json");
        var responseData = element._requestClient.Post<TIn, CouchDbResult>(requestData);
        return responseData;
    }
}