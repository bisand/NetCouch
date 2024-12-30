using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class DesignDoc : CouchDoc
{
    public DesignDoc()
    {
        Filters = new Dictionary<string, string>();
        Updates = new Dictionary<string, string>();
        Views = new Dictionary<string, View>();
    }

    [JsonPropertyName("language")]
    public string? Language { get; private set; }

    [JsonPropertyName("options")]
    public object? Options { get; private set; }

    [JsonPropertyName("filters")]
    public IDictionary<string, string> Filters { get; private set; }

    [JsonPropertyName("updates")]
    public IDictionary<string, string> Updates { get; private set; }

    [JsonPropertyName("views")]
    public IDictionary<string, View> Views { get; private set; }

    [JsonPropertyName("validate_doc_update")]
    public string? ValidateDocUpdate { get; private set; }

    [JsonPropertyName("autoupdate")]
    public bool? AutoUpdate { get; private set; }

    public void AddView(string name, View view)
    {
        Views ??= new Dictionary<string, View>();
        Views.Add(name, view);
    }

    public void RemoveView(string name)
    {
        if (Views == null)
            return;
        Views.Remove(name);
    }

    public void AddFilter(string name, string filter)
    {
        Filters ??= new Dictionary<string, string>();
        Filters.Add(name, filter);
    }

    public void RemoveFilter(string name)
    {
        if (Filters == null)
            return;
        Filters.Remove(name);
    }





}
