using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.Models.Couch.DesignDoc;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Biseth.Net.Settee.Linq.Old
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
            var queryResult = new CouchDbQueryExecuter<T>(_couchApi).Execute(result, queryString, viewQuery);


            // Try to extract the result.
            if (queryResult != null)
            {
                if (queryResult.DataDeserialized.Rows != null && queryResult.DataDeserialized.Rows.Count > 1)
                {
                    return queryResult.DataDeserialized.Rows.Select(x => x.Doc);
                }
                if (queryResult.DataDeserialized.Rows != null && queryResult.DataDeserialized.Rows.Count == 1)
                {
                    return queryResult.DataDeserialized.Rows.Select(x => x.Doc).FirstOrDefault();
                }
                return new List<ViewRow<T>>();
            }
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