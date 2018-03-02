using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class Block
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("previousblockhash")]
        public string Previousblockhash { get; set; }

        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("nextconsensus")]
        public string NextConsensus { get; set; }

        [JsonProperty("script")]
        public Script Script { get; set; }

        [JsonProperty("tx")]
        public List<BlockTx> Transaction { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("nextblockhash")]
        public string Nextblockhash { get; set; }
    }

    public class BlockTx
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("attributes")]
        public List<object> Attributes { get; set; }

        [JsonProperty("vin")]
        public List<object> Vin { get; set; }

        [JsonProperty("vout")]
        public List<object> Vout { get; set; }

        [JsonProperty("sys_fee")]
        public string SysFee { get; set; }

        [JsonProperty("net_fee")]
        public string NetFee { get; set; }

        [JsonProperty("scripts")]
        public List<object> Scripts { get; set; }

        [JsonProperty("asset")]
        public Asset Asset { get; set; }
    }
}