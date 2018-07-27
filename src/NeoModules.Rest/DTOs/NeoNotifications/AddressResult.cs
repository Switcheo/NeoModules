using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class AddressResult : NotificationResult
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }

        [JsonProperty("block")]
        public long Block { get; set; }

        [JsonProperty("tx")]
        public string Tx { get; set; }

        [JsonProperty("notify_type")]
        public string NotifyType { get; set; }

        [JsonProperty("addr_from")]
        public string AddrFrom { get; set; }

        [JsonProperty("addr_to")]
        public string AddrTo { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
