using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class ValueOut
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
