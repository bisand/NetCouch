using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.CouchDb.Api.Extensions;
using Biseth.Net.Settee.Models.Couch.DesignDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProvider<T> : QueryProvider
    {
        private readonly ICouchApi _couchApi;

        public CouchDbQueryProvider(ICouchApi couchApi)
        {
            _couchApi = couchApi;
        }

        public override string GetQueryText(Expression expression)
        {
            return Translate(expression).QueryText;
        }

        public override object Execute(Expression expression)
        {
            var result = Translate(expression);
            var viewQuery = new CouchDbViewQueryBuilder().Build(result);

            var queryString = viewQuery.Query;
            queryString += "&include_docs=true";

            // Query the database.
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
                designDoc.Views[result.ViewName] = new View{Map = viewQuery.View};

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


            // Try to extract the result.
            if (queryResult != null)
                return (queryResult.DataDeserialized.Rows ?? new List<ViewRow<T>>()).Select(x => x.Doc);
            
            // Something bad happened. We just return an empty result.
            return new List<ViewRow<T>>();
        }

        private static TranslateResult Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            var proj = (ProjectionExpression)new QueryBinder().Bind(expression);
            var result = new QueryFormatter().Format(proj.Source);
            return result;
        }
    }
}