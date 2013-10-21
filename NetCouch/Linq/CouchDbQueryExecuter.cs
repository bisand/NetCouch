using System.Net;
using Biseth.Net.Couch.Db.Api;
using Biseth.Net.Couch.Db.Api.Extensions;
using Biseth.Net.Couch.Http;
using Biseth.Net.Couch.Models.Couch.DesignDoc;

namespace Biseth.Net.Couch.Linq
{
    public class CouchDbQueryExecuter<T>
    {
        private readonly ICouchApi _couchApi;

        public CouchDbQueryExecuter(ICouchApi couchApi)
        {
            _couchApi = couchApi;
        }

        public ResponseData<ViewResponse<T>> Execute(CouchDbTranslation translation)
        {
            var queryResult =
                _couchApi.Root()
                         .Db(_couchApi.DefaultDatabase)
                         .DesignDoc(translation.DesignDocName)
                         .View(translation.ViewName, translation.ViewQuery.Query)
                         .Get<ViewResponse<T>>();


            // Create the view if it doess not exist.
            if (queryResult != null && queryResult.StatusCode == HttpStatusCode.NotFound)
            {
                // Retrieve the current design doc.
                var designDocResult =
                    _couchApi.Root()
                             .Db(_couchApi.DefaultDatabase)
                             .DesignDoc(translation.DesignDocName)
                             .Get<DesignDoc>();

                // Assign the view to the design doc object
                var designDoc = designDocResult.DataDeserialized;
                designDoc.Views[translation.ViewName] = new View {Map = translation.ViewQuery.View};

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
            return queryResult;
        }
    }
}