using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class ContractResult
    {
        [JsonProperty("current_height")]
        public long CurrentHeight { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("results")]
        public List<NotificationsContract> Results { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("page_len")]
        public long PageLen { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }
    }

    public class NotificationsContract
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

        [JsonProperty("addr_from", NullValueHandling = NullValueHandling.Ignore)]
        public string AddrFrom { get; set; }

        [JsonProperty("addr_to", NullValueHandling = NullValueHandling.Ignore)]
        public string AddrTo { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public string Amount { get; set; }
    }
}
