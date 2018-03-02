using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class Script
    {
        [JsonProperty("invocation")]
        public string Invocation { get; set; }

        [JsonProperty("verification")]
        public string Verification { get; set; }
    }
}
