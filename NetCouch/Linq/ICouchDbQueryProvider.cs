using System.Linq;

namespace Biseth.Net.Couch.Linq
{
    public interface ICouchDbQueryProvider : IQueryProvider
    {
        CouchDbTranslation QueryTranslation { get; }
    }
}