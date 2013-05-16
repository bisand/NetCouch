using System.Linq;

namespace Biseth.Net.Settee.Linq
{
    public interface ICouchDbQueryProvider : IQueryProvider
    {
        ICouchDbQueryGenerator QueryGenerator { get; }
        CouchDbTranslation QueryTranslation { get; }
    }
}