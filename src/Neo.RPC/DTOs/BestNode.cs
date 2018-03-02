using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class BestNode
    {
        [JsonProperty("net")]
        public string Net { get; set; }

        [JsonProperty("node")]
        public string NodeUrl { get; set; }
    }
}