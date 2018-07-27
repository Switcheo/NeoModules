using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoScan
{
    public class AddressHistory
    {
        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("txids")]
        public IList<TxId> Txids { get; set; }

        [JsonProperty("tx_count")]
        public int TxCount { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("claimed")]
        public IList<ClaimedElement> Claimed { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("balance")]
        public IList<Balance> Balance { get; set; }

        public static AddressHistory FromJson(string json) => JsonConvert.DeserializeObject<AddressHistory>(json, Utils.Settings);
    }
}