using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class Transaction
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("attributes")]
        public List<object> Attributes { get; set; }

        [JsonProperty("vin")]
        public List<ValueIn> Vin { get; set; }

        [JsonProperty("vout")]
        public List<ValueOut> Vout { get; set; }

        [JsonProperty("sys_fee")]
        public string SysFee { get; set; }

        [JsonProperty("net_fee")]
        public string NetFee { get; set; }

        [JsonProperty("scripts")]
        public List<Script> Scripts { get; set; }

        [JsonProperty("blockhash")]
        public string Blockhash { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("blocktime")]
        public int Blocktime { get; set; }
    }
}