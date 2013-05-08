using System;
using System.Linq;
using System.Linq.Expressions;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.Linq.Old;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProvider<T> : ICouchDbQueryProvider
    {
        public CouchDbQueryProvider(ICouchDbQueryGenerator queryGenerator, CouchDbTranslation queryTranslation)
        {
            QueryGenerator = queryGenerator;
            QueryTranslation = queryTranslation;
        }

        public CouchDbQueryProvider(ICouchApi couchApi)
        {
            throw new NotImplementedException();
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            return GetQueryProviderProcessor<T>().Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult) Execute(expression);
        }

        public ICouchDbQueryGenerator QueryGenerator { get; private set; }
        public CouchDbTranslation QueryTranslation { get; private set; }

        private CouchDbQueryProviderProcessor<TResult> GetQueryProviderProcessor<TResult>()
        {
            return new CouchDbQueryProviderProcessor<TResult>(QueryGenerator, QueryTranslation);
        }
    }
}