using System.Collections.Generic;

namespace NetCouch.Linq;

public class CouchDbTranslation
{
    public CouchDbTranslation()
    {
        QueryProperties = [];
        QueryValues = [];
        Statements = [];
        ViewQuery = new ViewAndQuery();
        Skip = 0;
        Limit = 100;
    }

    public string? QueryText { get; set; }
    public string? DesignDocName { get; set; }
    public string? ViewName { get; set; }
    public List<string> QueryProperties { get; set; }
    public List<string> QueryValues { get; set; }
    public List<Statement> Statements { get; set; }
    public ViewAndQuery ViewQuery { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}