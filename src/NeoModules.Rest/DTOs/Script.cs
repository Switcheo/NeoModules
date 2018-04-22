using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class Script
    {
        [JsonProperty("verification")]
        public string Verification { get; set; }

        [JsonProperty("invocation")]
        public string Invocation { get; set; }
    }
}
