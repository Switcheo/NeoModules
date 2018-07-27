using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class ContractResult : BlockResult
    {
        [JsonProperty("addr_from", NullValueHandling = NullValueHandling.Ignore)]
        public string AddrFrom { get; set; }

        [JsonProperty("addr_to", NullValueHandling = NullValueHandling.Ignore)]
        public string AddrTo { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public string Amount { get; set; }
    }
}
