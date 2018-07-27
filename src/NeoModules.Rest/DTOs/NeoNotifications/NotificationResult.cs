using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class NotificationResult
    {
        [JsonProperty("current_height")]
        public long CurrentHeight { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("results")]
        public List<ContractResult> Results { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("page_len")]
        public long PageLen { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }
    }
}
