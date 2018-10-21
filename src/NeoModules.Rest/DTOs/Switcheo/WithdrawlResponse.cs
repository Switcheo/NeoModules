using System;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class WithdrawlResponse
    {
        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("asset_id")]
        public string AssetId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("blockchain")]
        public string Blockchain { get; set; }

        [JsonProperty("reason_code")]
        public long ReasonCode { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("transaction_hash")]
        public object TransactionHash { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("contract_hash")]
        public string ContractHash { get; set; }

        public static WithdrawlResponse FromJson(string json) => JsonConvert.DeserializeObject<WithdrawlResponse>(json, Utils.Settings);
    }
}
