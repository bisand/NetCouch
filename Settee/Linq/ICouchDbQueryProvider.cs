using System.Collections.Generic;
using System.Linq;
using Biseth.Net.Settee.Linq.Old;

namespace Biseth.Net.Settee.Linq
{
    public interface ICouchDbQueryProvider : IQueryProvider
    {
        ICouchDbQueryGenerator QueryGenerator { get; }
        CouchDbTranslation QueryTranslation { get; }
    }
}