using System.Linq;

namespace Biseth.Net.Settee.Linq
{
    public interface ICouchDbQueryable<out T> : IOrderedQueryable<T>
    {
    }
}