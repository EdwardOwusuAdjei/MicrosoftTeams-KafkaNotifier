using Newtonsoft.Json;


namespace AutoLagNotifier.Models.Response
{

    public partial class LagResponseModel
    {
        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("request")]
        public Request Request { get; set; }
    }

    public partial class Request
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("cluster")]
        public string Cluster { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("status")]
        public string StatusStatus { get; set; }

        [JsonProperty("complete")]
        public long Complete { get; set; }

        [JsonProperty("partitions")]
        public Maxlag[] Partitions { get; set; }

        [JsonProperty("partition_count")]
        public long PartitionCount { get; set; }

        [JsonProperty("maxlag")]
        public Maxlag Maxlag { get; set; }

        [JsonProperty("totallag")]
        public long Totallag { get; set; }
    }

    public partial class Maxlag
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("partition")]
        public long Partition { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("start")]
        public End Start { get; set; }

        [JsonProperty("end")]
        public End End { get; set; }

        [JsonProperty("current_lag")]
        public long CurrentLag { get; set; }

        [JsonProperty("complete")]
        public double Complete { get; set; }
    }

    public partial class End
    {
        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("lag")]
        public long Lag { get; set; }
    }
}