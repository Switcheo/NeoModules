using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class ValueIn
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("vout")]
        public int Vout { get; set; }
    }
}
