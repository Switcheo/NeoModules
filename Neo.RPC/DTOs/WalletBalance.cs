using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class WalletBalance
    {
        [JsonProperty("Balance")]
        public string Balance { get; set; }

        [JsonProperty("Confirmed")]
        public string Confirmed { get; set; }
    }
}
