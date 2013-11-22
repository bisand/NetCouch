using System;
using System.Runtime.Serialization;

namespace Biseth.Net.Couch.Models.Couch.Database
{
    [DataContract]
    public class DatabaseInfo
    {
        [DataMember(Name = "committed_update_seq", EmitDefaultValue = false)]
        public int CommittedUpdateSeq { get; set; }

        [DataMember(Name = "compact_running", EmitDefaultValue = false)]
        public bool CompactRunning { get; set; }

        [DataMember(Name = "db_name", EmitDefaultValue = false)]
        public string DbName { get; set; }

        [DataMember(Name = "disk_format_version", EmitDefaultValue = false)]
        public int DiskFormatVersion { get; set; }

        [DataMember(Name = "disk_size", EmitDefaultValue = false)]
        public int DiskSize { get; set; }

        [DataMember(Name = "doc_count", EmitDefaultValue = false)]
        public int DocCount { get; set; }

        [DataMember(Name = "doc_del_count", EmitDefaultValue = false)]
        public int DocDelCount { get; set; }

        [DataMember(Name = "instance_start_time", EmitDefaultValue = false)]
        public string InstanceStartTime { get; set; }

        [DataMember(Name = "purge_seq", EmitDefaultValue = false)]
        public int PurgeSeq { get; set; }

        [DataMember(Name = "update_seq", EmitDefaultValue = false)]
        public int UpdateSeq { get; set; }
    }
}