using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Biseth.Net.Couch.Db.Api;
using Biseth.Net.Couch.Models.Couch.DesignDoc;

namespace Biseth.Net.Couch.Linq
{
    public class CouchDbQueryProvider<T> : ICouchDbQueryProvider
    {
        private readonly ICouchApi _couchApi;
        private readonly HashSet<dynamic> _trackedEntities;

        public CouchDbQueryProvider(ICouchApi couchApi, CouchDbTranslation queryTranslation, HashSet<dynamic> trackedEntities)
        {
            _couchApi = couchApi;
            _trackedEntities = trackedEntities;
            QueryTranslation = queryTranslation;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new CouchDbQuery<TElement>(this, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable) Activator.CreateInstance(
                    typeof (CouchDbQuery<>).MakeGenericType(elementType),
                    new object[] {this, expression});
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public object Execute(Expression expression)
        {
            //var translation = GetQueryProviderProcessor<T>().Execute(expression);
            var translation = new CouchDbVisitor<T>(QueryTranslation).Execute(expression);
            translation.ViewQuery = new CouchDbViewQueryBuilder(translation).Build();
            translation.ViewQuery.Query += "&include_docs=true";
            var queryResult = new CouchDbQueryExecuter<T>(_couchApi).Execute(translation);

            // Try to extract the result.
            if (queryResult != null)
            {
                foreach (var row in queryResult.DataDeserialized.Rows)
                {
                    _trackedEntities.Add(row.Doc);
                }

                if (queryResult.DataDeserialized.Rows != null && queryResult.DataDeserialized.Rows.Count > 1)
                {
                    return queryResult.DataDeserialized.Rows.Select(x => x.Doc.Entity);
                }
                if (queryResult.DataDeserialized.Rows != null && queryResult.DataDeserialized.Rows.Count == 1)
                {
                    return queryResult.DataDeserialized.Rows.Select(x => x.Doc.Entity).FirstOrDefault();
                }
                return new List<ViewRow<T>>();
            }

            // Something bad happened. We just return an empty result.
            return new List<ViewRow<T>>();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult) Execute(expression);
        }

        public CouchDbTranslation QueryTranslation { get; private set; }

        private CouchDbQueryProviderProcessor<TResult> GetQueryProviderProcessor<TResult>()
        {
            return new CouchDbQueryProviderProcessor<TResult>(QueryTranslation);
        }
    }
}