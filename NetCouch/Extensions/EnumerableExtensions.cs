using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Biseth.Net.Couch.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable Cast(this IEnumerable self, Type innerType)
        {
            var methodInfo = typeof(Enumerable).GetMethod("Cast");
            var genericMethod = methodInfo.MakeGenericMethod(innerType);
            return genericMethod.Invoke(null, new[] { self }) as IEnumerable;
        }
    }
}