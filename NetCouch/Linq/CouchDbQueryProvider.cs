using System.Linq.Expressions;
using System.Reflection;
using NetCouch.Db.Api;

namespace NetCouch.Linq;

public class CouchDbQueryProvider<T> : ICouchDbQueryProvider
{
    private readonly ICouchApi _couchApi;
    private readonly List<T> _trackedEntities;

    public CouchDbQueryProvider(ICouchApi couchApi, CouchDbTranslation queryTranslation, List<dynamic> trackedDocuments)
    {
        _couchApi = couchApi;
        _trackedEntities = new List<T>();
        QueryTranslation = queryTranslation;
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new CouchDbQueryable<TElement>(this, expression);
    }

    public IQueryable CreateQuery(Expression expression)
    {
        var elementType = TypeSystem.GetElementType(expression.Type) ?? throw new InvalidOperationException($"No element type by the name of {expression.Type} found.");
        try
        {
            return (IQueryable)Activator.CreateInstance(
                typeof(CouchDbQueryable<>).MakeGenericType(elementType),
                new object[] { this, expression })!;
        }
        catch (TargetInvocationException tie)
        {
            if (tie.InnerException != null)
            {
                throw tie.InnerException;
            }
            throw;
        }
    }

    public object Execute(Expression expression)
    {
        var translation = new CouchDbVisitor<T>(QueryTranslation).Execute(expression);
        translation.ViewQuery = new CouchDbViewQueryBuilder<T>(translation).Build();
        translation.ViewQuery.Query += "&include_docs=true";
        var queryResult = new CouchDbQueryExecuter<T>(_couchApi).Execute(translation);

        // Try to extract the result.
        if (queryResult != null && queryResult.Body != null && queryResult.Body.Rows != null)
        {
            foreach (var row in queryResult.Body.Rows)
            {
                if (row.Doc != null)
                    _trackedEntities.Add(row.Doc);
            }

            if (_trackedEntities.Count > 1)
                return _trackedEntities;
        }
        return new List<T>();
    }

    public TResult Execute<TResult>(Expression expression)
    {
        var translation = new CouchDbVisitor<T>(QueryTranslation).Execute(expression);
        translation.ViewQuery = new CouchDbViewQueryBuilder<T>(translation).Build();
        translation.ViewQuery.Query += "&include_docs=true";
        var queryResult = new CouchDbQueryExecuter<T>(_couchApi).Execute(translation);

        if (queryResult != null && queryResult.Body != null && queryResult.Body.Rows != null)
        {
            foreach (var row in queryResult.Body.Rows)
            {
                if (row.Doc != null)
                    _trackedEntities.Add(row.Doc);
            }
            if (expression is MethodCallExpression methodCall)
            {
                if (methodCall.Method.Name == nameof(Queryable.FirstOrDefault))
                {
                    if (_trackedEntities.Count > 1)
                        return (TResult)(object)_trackedEntities.FirstOrDefault()!;
                }
            }
        }

        return (TResult)Execute(expression);
    }

    public CouchDbTranslation QueryTranslation { get; set; }

    private CouchDbQueryProviderProcessor<TResult> GetQueryProviderProcessor<TResult>()
    {
        return new CouchDbQueryProviderProcessor<TResult>(QueryTranslation);
    }
}