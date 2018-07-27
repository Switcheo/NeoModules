using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class BlockResult : NotificationResult
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }

        [JsonProperty("block")]
        public long Block { get; set; }

        [JsonProperty("tx")]
        public string Tx { get; set; }

        [JsonProperty("notify_type")]
        public string NotifyType { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
