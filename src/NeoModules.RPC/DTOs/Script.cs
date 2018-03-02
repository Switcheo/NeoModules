using Newtonsoft.Json;

namespace NeoModules.RPC.DTOs
{
    public class Script
    {
        [JsonProperty("invocation")]
        public string Invocation { get; set; }

        [JsonProperty("verification")]
        public string Verification { get; set; }
    }
}
