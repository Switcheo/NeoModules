using Newtonsoft.Json;

namespace NeoModules.RPC.DTOs
{
    public class BlockHeader
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("previousblockhash")]
        public string Previousblockhash { get; set; }

        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("nextconsensus")]
        public string Nextconsensus { get; set; }

        [JsonProperty("script")]
        public Script Script { get; set; }

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        [JsonProperty("nextblockhash")]
        public string Nextblockhash { get; set; }
    }
}
