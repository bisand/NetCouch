using System.Net;
using NetCouch.Db.Api;
using NetCouch.Db.Api.Extensions;
using NetCouch.Http;
using NetCouch.Models.Couch.DesignDoc;

namespace NetCouch.Linq;

public class CouchDbQueryExecuter<T>
{
    private readonly ICouchApi _couchApi;

    public CouchDbQueryExecuter(ICouchApi couchApi)
    {
        _couchApi = couchApi;
    }

    public ResponseData<ViewResponse<T>> Execute(CouchDbTranslation translation)
    {
        ArgumentNullException.ThrowIfNull(nameof(translation));
        ArgumentException.ThrowIfNullOrWhiteSpace(translation.DesignDocName, nameof(translation.DesignDocName));
        ArgumentException.ThrowIfNullOrWhiteSpace(translation.ViewName, nameof(translation.ViewName));
        ArgumentNullException.ThrowIfNull(translation.ViewQuery, nameof(translation.ViewQuery));
        ArgumentException.ThrowIfNullOrWhiteSpace(translation.ViewQuery.Query, nameof(translation.ViewQuery.Query));

        var queryResult =
            _couchApi.Root()
                     .Db(_couchApi.DefaultDatabase)
                     .DesignDoc(translation.DesignDocName)
                     .View(translation.ViewName, translation.ViewQuery.Query)
                     .Get<ViewResponse<T>>();


        // Create the view if it doess not exist.
        if (queryResult != null && queryResult.StatusCode == HttpStatusCode.NotFound)
        {
            ArgumentNullException.ThrowIfNull(translation.ViewQuery.View, nameof(translation.ViewQuery.View));

            // Retrieve the current design doc.
            var designDocResult =
                _couchApi.Root()
                         .Db(_couchApi.DefaultDatabase)
                         .DesignDoc(translation.DesignDocName)
                         .Get<DesignDoc>();

            // Assign the view to the design doc object
            var designDoc = designDocResult.Body ?? new DesignDoc();
            designDoc.Views[translation.ViewName] = new View { Map = translation.ViewQuery.View };

            // Save the design doc back to the server.
            var responseData =
                _couchApi.Root()
                         .Db(_couchApi.DefaultDatabase)
                         .DesignDoc(translation.DesignDocName)
                         .Put<DesignDoc, object>(designDoc);

            // If the design doc was successfully created, we re-run the query.
            if (responseData != null && responseData.StatusCode == HttpStatusCode.Created)
            {
                queryResult =
                    _couchApi.Root()
                             .Db(_couchApi.DefaultDatabase)
                             .DesignDoc(translation.DesignDocName)
                             .View(translation.ViewName, translation.ViewQuery.Query)
                             .Get<ViewResponse<T>>();
            }
        }
        return queryResult ?? new ResponseData<ViewResponse<T>>();
    }
}