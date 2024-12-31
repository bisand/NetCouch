using System.Collections;
using System.Linq.Expressions;

namespace NetCouch.Linq;

public class CouchDbQueryable<T> : ICouchDbQueryable<T>
{
    private readonly Expression _expression;
    private readonly IQueryProvider _provider;

    public CouchDbQueryable(CouchDbQueryProvider<T> provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _expression = Expression.Constant(this);
    }

    public CouchDbQueryable(ICouchDbQueryProvider provider, Expression expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }
        if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
        {
            throw new ArgumentOutOfRangeException(nameof(expression));
        }
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _expression = expression;
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

    public Expression Expression => _expression;
    public IQueryProvider Provider => _provider;
    public Type ElementType => typeof(T);
}