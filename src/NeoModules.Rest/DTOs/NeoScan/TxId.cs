using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoScan
{
    public class TxId
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("balance")]
        public IList<TxidBalance> Balance { get; set; }

        [JsonProperty("asset_moved")]
        public string AssetMoved { get; set; }

        [JsonProperty("amount_moved")]
        public double AmountMoved { get; set; }
    }

    public class TxidBalance
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }
    }
}
