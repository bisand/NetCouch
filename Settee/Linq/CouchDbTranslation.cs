using System.Collections.Generic;
using Biseth.Net.Settee.Linq.Old;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbTranslation
    {
        public string QueryText { get; set; }
        public string DesignDocName { get; set; }
        public string ViewName { get; set; }
        public List<string> QueryProperties { get; set; }
        public List<string> QueryValues { get; set; }
        public List<Statement> Statements { get; set; }
        public ViewAndQuery ViewQuery { get; set; }
}
}