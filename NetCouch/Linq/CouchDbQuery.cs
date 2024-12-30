using System.Collections;
using System.Linq.Expressions;

namespace NetCouch.Linq;

public class CouchDbQuery<T> : ICouchDbQueryable<T>
{
    public CouchDbQuery(CouchDbQueryProvider<T> provider)
    {
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        Expression = Expression.Constant(this);
        ElementType = typeof(T);
    }

    public CouchDbQuery(ICouchDbQueryProvider provider, Expression expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }
        if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
        {
            throw new ArgumentOutOfRangeException(nameof(expression));
        }
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        Expression = expression;
        ElementType = typeof(T);
    }

    public IEnumerator<T> GetEnumerator()
    {
        var result = Provider.Execute(Expression);
        if (result is IEnumerable<T> enumerable)
            return enumerable.GetEnumerator();

        return new List<T> { (T)result! }.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Expression Expression { get; private set; }
    public Type ElementType { get; private set; }
    public IQueryProvider Provider { get; private set; }
}