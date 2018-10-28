using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class OrderRequest
    {
        [JsonProperty("blockchain", Order = 1)]
        public string Blockchain { get; set; }

        [JsonProperty("contract_hash", Order = 2)]
        public string ContractHash { get; set; }

        [JsonProperty("order_type", Order = 3)]
        public string OrderType { get; set; }

        //[JsonProperty("otc_address", Order = 4)]
        //public string OtcAddress { get; set; }

        [JsonProperty("pair", Order = 4)]
        public string Pair { get; set; }

        [JsonProperty("price", Order = 5)]
        public string Price { get; set; }

        [JsonProperty("side", Order = 6)]
        public string Side { get; set; }

        [JsonProperty("timestamp", Order = 7)]
        public long Timestamp { get; set; }

        [JsonProperty("use_native_tokens", Order = 8)]
        public bool UseNativeTokens { get; set; }

        [JsonProperty("want_amount", Order = 9)]
        public string WantAmount { get; set; }
    }
}
