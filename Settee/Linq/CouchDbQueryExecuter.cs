using System.Net;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.CouchDb.Api.Extensions;
using Biseth.Net.Settee.Http;
using Biseth.Net.Settee.Linq.Old;
using Biseth.Net.Settee.Models.Couch.DesignDoc;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryExecuter<T>
    {
        private readonly ICouchApi _couchApi;

        public CouchDbQueryExecuter(ICouchApi couchApi)
        {
            _couchApi = couchApi;
        }

        public ResponseData<ViewResponse<T>> Execute(CouchDbTranslation result, string queryString, ViewAndQuery viewAndQuery)
        {
            var queryResult =
                _couchApi.Root()
                         .Db(_couchApi.DefaultDatabase)
                         .DesignDoc(result.DesignDocName)
                         .View(result.ViewName, queryString)
                         .Get<ViewResponse<T>>();


            // Create the view if it doess not exist.
            if (queryResult != null && queryResult.StatusCode == HttpStatusCode.NotFound)
            {
                // Retrieve the current design doc.
                var designDocResult =
                    _couchApi.Root()
                             .Db(_couchApi.DefaultDatabase)
                             .DesignDoc(result.DesignDocName)
                             .Get<DesignDoc>();

                // Assign the view to the design doc object
                var designDoc = designDocResult.DataDeserialized;
                designDoc.Views[result.ViewName] = new View { Map = viewAndQuery.View };

                // Save the design doc back to the server.
                var responseData =
                    _couchApi.Root()
                             .Db(_couchApi.DefaultDatabase)
                             .DesignDoc(result.DesignDocName)
                             .Put<DesignDoc, object>(designDoc);

                // If the design doc was successfully created, we re-run the query.
                if (responseData != null && responseData.StatusCode == HttpStatusCode.Created)
                {
                    queryResult =
                        _couchApi.Root()
                                 .Db(_couchApi.DefaultDatabase)
                                 .DesignDoc(result.DesignDocName)
                                 .View(result.ViewName, queryString)
                                 .Get<ViewResponse<T>>();
                }
            }
            return queryResult;
        }

    }
}