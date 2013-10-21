using System.Linq;

namespace Biseth.Net.Couch.Linq
{
    public interface ICouchDbQueryable<out T> : IOrderedQueryable<T>
    {
    }
}