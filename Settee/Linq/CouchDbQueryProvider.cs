using System;
using System.Linq;
using System.Linq.Expressions;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProvider : IQueryProvider
    {
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
            return TerraServerQueryContext.Execute(expression, false);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            bool IsEnumerable = (typeof(TResult).Name == "IEnumerable`1");

            return (TResult)TerraServerQueryContext.Execute(expression, IsEnumerable);
        }
    }

    public class TerraServerQueryContext
    {
        public static object Execute(Expression expression, bool b)
        {
            throw new NotImplementedException();
        }
    }
}