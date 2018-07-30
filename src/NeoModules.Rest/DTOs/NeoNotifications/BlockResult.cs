using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class BlockResult
    {
        [JsonProperty("current_height")]
        public long CurrentHeight { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("results")]
        public List<NotificationsBlock> Results { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("page_len")]
        public long PageLen { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }
    }

    public class NotificationsBlock
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
