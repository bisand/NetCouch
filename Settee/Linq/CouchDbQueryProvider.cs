using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Biseth.Net.Settee.CouchDb.Api;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProvider<T> : ICouchDbQueryProvider
    {
        private readonly ICouchApi _couchApi;

        public CouchDbQueryProvider(ICouchDbQueryGenerator queryGenerator, CouchDbTranslation queryTranslation)
        {
            QueryGenerator = queryGenerator;
            QueryTranslation = queryTranslation;
        }

        public CouchDbQueryProvider(ICouchApi couchApi)
        {
            _couchApi = couchApi;
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