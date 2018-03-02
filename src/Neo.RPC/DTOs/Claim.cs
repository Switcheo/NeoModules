using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public struct Claim
    {
        [JsonProperty("claim")]
        public int ClaimNumber { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("sysfee")]
        public int Sysfee { get; set; }

        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
