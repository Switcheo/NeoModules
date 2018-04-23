using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class NeonAddress
    {
        [JsonProperty("txids")]
        public IList<TxId> TxIds { get; set; }

        [JsonProperty("tx_count")]
        public long TxCount { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("claimed")]
        public IList<ClaimedElement> Claimed { get; set; }

        [JsonProperty("balance")]
        public IList<NeonBalance> Balance { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static NeonAddress FromJson(string json)=> JsonConvert.DeserializeObject<NeonAddress>(json, Utils.Settings);
    }

    public class NeonBalance
    {
        [JsonProperty("unspent")]
        public IList<Unspent> Unspent { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
