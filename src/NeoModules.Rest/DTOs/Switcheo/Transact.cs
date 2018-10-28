using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class Transact
    {
        [JsonProperty("amount", Order = 1)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Amount { get; set; }

        [JsonProperty("asset_id", Order = 2)]
        public string AssetId { get; set; }

        [JsonProperty("blockchain", Order = 3)]
        public string Blockchain { get; set; }

        [JsonProperty("contract_hash", Order = 4)]
        public string ContractHash { get; set; }

        [JsonProperty("timestamp", Order = 5)]
        public long Timestamp { get; set; }
    }
}
