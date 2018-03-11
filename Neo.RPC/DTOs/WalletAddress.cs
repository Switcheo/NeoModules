using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class WalletAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("haskey")]
        public bool HasKey { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("watchonly")]
        public bool WatchOnly { get; set; }
    }
}
