using System.Collections.Generic;
using Biseth.Net.Settee.Linq.Old;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbTranslation
    {
        internal string QueryText;
        internal string DesignDocName;
        internal string ViewName;
        public List<string> QueryProperties { get; set; }
        public List<string> QueryValues { get; set; }
        public List<Statement> Statements { get; set; }
    }
}