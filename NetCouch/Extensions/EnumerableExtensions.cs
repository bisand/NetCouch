using System.Collections;

namespace NetCouch.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable? Cast(this IEnumerable self, Type innerType)
    {
        var methodInfo = typeof(Enumerable).GetMethod("Cast") ?? throw new InvalidOperationException("The method 'Cast' could not be found.");
        var genericMethod = methodInfo.MakeGenericMethod(innerType);
        return genericMethod.Invoke(null, [self]) as IEnumerable;
    }
}