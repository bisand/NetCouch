using System;
using System.Linq;
using System.Linq.Expressions;
using Biseth.Net.Settee.CouchDb.Api;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProvider : IQueryProvider
    {
        private readonly ICouchApi _couchApi;

        public CouchDbQueryProvider(ICouchApi couchApi)
        {
            _couchApi = couchApi;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable) Activator.CreateInstance(typeof (CouchDbQuery<>).MakeGenericType(elementType), new object[] {this, expression});
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new CouchDbQuery<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return CouchDbQueryContext.Execute(_couchApi, expression, false);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var execute = Execute(expression);
            return (TResult) execute;
        }
    }
}