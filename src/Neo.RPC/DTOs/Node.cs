using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class Node
    {
        [JsonProperty("block_height")]
        public int? BlockHeight { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("time")]
        public double? Time { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}