using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class Transact
    {
        [JsonProperty("blockchain")]
        public string Blockchain { get; set; }

        [JsonProperty("asset_id")]
        public string AssetId { get; set; }

        [JsonProperty("amount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Amount { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("contract_hash")]
        public string ContractHash { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; } //Do not include this in the parameters to be signed.
    }
}
