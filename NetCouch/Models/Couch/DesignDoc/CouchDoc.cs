using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.DesignDoc;

public class CouchDoc
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("_rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("_deleted")]
    public bool? Deleted { get; set; }

    [JsonPropertyName("_attachments")]
    public IDictionary<string, Attachment>? Attachments { get; set; }

    [JsonPropertyName("_conflicts")]
    public string[]? Conflicts { get; set; }

    [JsonPropertyName("_deleted_conflicts")]
    public string[]? DeletedConflicts { get; set; }

    [JsonPropertyName("_local_seq")]
    public string? LocalSeq { get; set; }

    [JsonPropertyName("_revs_info")]
    public RevInfo[]? RevsInfo { get; set; }

    [JsonPropertyName("_revisions")]
    public Revisions? Revisions { get; set; }
}
