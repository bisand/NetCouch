using System;
using System.Linq.Expressions;
using Biseth.Net.Settee.Linq.Old;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProviderProcessor<T>
    {
        public CouchDbQueryProviderProcessor(ICouchDbQueryGenerator queryGenerator, CouchDbTranslation queryTranslation, ViewAndQuery viewQuery)
        {
            
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}