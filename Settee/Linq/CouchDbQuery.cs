using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQuery<T> : IOrderedQueryable<T>
    {
        public CouchDbQuery(IQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof (IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            Provider = provider;
            Expression = expression;
        }

        public CouchDbQuery()
        {
            Expression = Expression.Constant(this);
            Provider = new CouchDbQueryProvider();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<IEnumerable>(Expression)).GetEnumerator();
        }

        public Expression Expression { get; private set; }

        public Type ElementType
        {
            get { return typeof (T); }
        }

        public IQueryProvider Provider { get; private set; }
    }
}