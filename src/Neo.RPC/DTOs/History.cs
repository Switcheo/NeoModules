using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class History
    {
        [JsonProperty("GAS")]
        public double Gas { get; set; }

        [JsonProperty("NEO")]
        public int Neo { get; set; }

        [JsonProperty("block_index")]
        public int BlockIndex { get; set; }

        [JsonProperty("gas_sent")]
        public bool GasSent { get; set; }

        [JsonProperty("neo_sent")]
        public bool NeoSent { get; set; }

        [JsonProperty("txid")]
        public string TransactionId { get; set; }
    }
}