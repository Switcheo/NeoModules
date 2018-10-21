using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class OrderRequest
    {
        [JsonProperty("pair")]
        public string Pair { get; set; }

        [JsonProperty("blockchain")]
        public string Blockchain { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("want_amount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long WantAmount { get; set; }

        [JsonProperty("use_native_tokens")]
        public bool UseNativeTokens { get; set; }

        [JsonProperty("order_type")]
        public string OrderType { get; set; }

        [JsonProperty("otc_address")]
        public string OtcAddress { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("contract_hash")]
        public string ContractHash { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
