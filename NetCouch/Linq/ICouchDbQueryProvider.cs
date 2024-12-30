using System.Linq;

namespace NetCouch.Linq;

public interface ICouchDbQueryProvider : IQueryProvider
{
    CouchDbTranslation QueryTranslation { get; }
}