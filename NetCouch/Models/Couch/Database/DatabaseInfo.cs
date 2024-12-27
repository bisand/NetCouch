using System.Text.Json.Serialization;

namespace NetCouch.Models.Couch.Database
{
    public class DatabaseInfo : InternalResult
    {
        [JsonPropertyName("committed_update_seq")]
        public int CommittedUpdateSeq { get; set; }

        [JsonPropertyName("compact_running")]
        public bool CompactRunning { get; set; }

        [JsonPropertyName("db_name")]
        public string? DbName { get; set; }

        [JsonPropertyName("disk_format_version")]
        public int DiskFormatVersion { get; set; }

        [JsonPropertyName("disk_size")]
        public int DiskSize { get; set; }

        [JsonPropertyName("doc_count")]
        public int DocCount { get; set; }

        [JsonPropertyName("doc_del_count")]
        public int DocDelCount { get; set; }

        [JsonPropertyName("instance_start_time")]
        public string? InstanceStartTime { get; set; }

        [JsonPropertyName("purge_seq")]
        public string? PurgeSeq { get; set; }

        [JsonPropertyName("update_seq")]
        public string? UpdateSeq { get; set; }

        [JsonPropertyName("sizes")]
        public Sizes? Sizes { get; set; }

        [JsonPropertyName("props")]
        public Dictionary<string, object>? Props { get; set; }

        [JsonPropertyName("cluster")]
        public Cluster? Cluster { get; set; }
    }
}