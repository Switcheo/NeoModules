using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoScan
{
    public class Transaction
    {
        [JsonProperty("vouts")]
        public IList<Vout> Vouts { get; set; }

        [JsonProperty("vin")]
        public IList<Vin> Vins { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("sys_fee")]
        public long SysFee { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("scripts")]
        public IList<Script> Scripts { get; set; }

        [JsonProperty("pubkey")]
        public string Pubkey { get; set; }

        [JsonProperty("nonce")]
        public long? Nonce { get; set; }

        [JsonProperty("net_fee")]
        public long NetFee { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("contract")]
        public object Contract { get; set; }

        [JsonProperty("claims")]
        public IList<Vin> Claims { get; set; }

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("block_hash")]
        public string BlockHash { get; set; }

        [JsonProperty("attributes")]
        public IList<Atribute> Attributes { get; set; }

        [JsonProperty("asset")]
        public object Asset { get; set; }

        public static Transaction FromJson(string json) => JsonConvert.DeserializeObject<Transaction>(json, Utils.Settings);
    }

    public class Transactions
    {
        public static IList<Transaction> FromJson(string json) => JsonConvert.DeserializeObject<IList<Transaction>>(json, Utils.Settings);
    }

    public class Vout
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("address_hash")]
        public string Address { get; set; }
    }

    public class Vin
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("address_hash")]
        public string AddressHash { get; set; }
    }

    public class Atribute
    {
        [JsonProperty("usage")]
        public string Usage { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}