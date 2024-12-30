using System.Linq;

namespace NetCouch.Linq;

public interface ICouchDbQueryable<out T> : IOrderedQueryable<T>
{
}