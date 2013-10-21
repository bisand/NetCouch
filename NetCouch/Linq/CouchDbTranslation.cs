using System.Collections.Generic;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbTranslation
    {
        public CouchDbTranslation()
        {
            QueryProperties = new List<string>();
            QueryValues = new List<string>();
            Statements = new List<Statement>();
            ViewQuery = new ViewAndQuery();
        }

        public string QueryText { get; set; }
        public string DesignDocName { get; set; }
        public string ViewName { get; set; }
        public List<string> QueryProperties { get; set; }
        public List<string> QueryValues { get; set; }
        public List<Statement> Statements { get; set; }
        public ViewAndQuery ViewQuery { get; set; }
}
}