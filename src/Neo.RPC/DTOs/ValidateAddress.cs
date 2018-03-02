using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class ValidateAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("isvalid")]
        public bool IsValid { get; set; }
    }
}
