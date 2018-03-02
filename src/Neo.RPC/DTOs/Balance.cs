using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class Balance
    {
        [JsonProperty("GAS")]
        public Coin Gas { get; set; }

        [JsonProperty("NEO")]
        public Coin Neo { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("net")]
        public string Net { get; set; }
    }
}
