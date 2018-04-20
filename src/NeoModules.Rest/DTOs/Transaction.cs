using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
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
        public string SysFee { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("scripts")]
        public IList<Script> Scripts { get; set; }

        [JsonProperty("script")]
        public object Script { get; set; }

        [JsonProperty("pubkey")]
        public string Pubkey { get; set; }

        [JsonProperty("nonce")]
        public long Nonce { get; set; }

        [JsonProperty("net_fee")]
        public string NetFee { get; set; }

        [JsonProperty("descriptors")]
        public object Descriptors { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("contract")]
        public object Contract { get; set; }

        [JsonProperty("claims")]
        public IList<Claim> Claims { get; set; }

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("block_hash")]
        public string BlockHash { get; set; }

        [JsonProperty("attributes")]
        public IList<object> Attributes { get; set; }

        [JsonProperty("asset_moved")]
        public string AssetMoved { get; set; }

        [JsonProperty("asset")]
        public object Asset { get; set; }

        public static Transaction FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<Transaction>(json,settings);
        }
    }

    public class Vout
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class Vin
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("txid")]
        public float Txid { get; set; }

        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("address_hash")]
        public string AddressHash { get; set; }
    }

    public class Script
    {
        [JsonProperty("verification")]
        public string Verification { get; set; }

        [JsonProperty("invocation")]
        public string Invocation { get; set; }
    }

    public class Claim
    {
        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("address_hash")]
        public string AddressHash { get; set; }
    }
}