using System.Linq;

namespace Biseth.Net.Settee.Linq
{
    public interface ICouchDbQueryProvider : IQueryProvider
    {
        CouchDbTranslation QueryTranslation { get; }
    }
}