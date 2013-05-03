using System.Linq.Expressions;

namespace Biseth.Net.Settee.Linq.Old
{
    public class CouchDbQuery<T> : Query<T>
    {
        public CouchDbQuery(CouchDbQueryProvider<T> provider)
            : base(provider)
        {
        }

        public CouchDbQuery(CouchDbQueryProvider<T> provider, Expression expression)
            : base(provider, expression)
        {
        }
    }
}