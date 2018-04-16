using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class Transaction
    {
        [JsonProperty("vouts")]
        public List<Vout> Vouts { get; set; }

        [JsonProperty("vin")]
        public List<Vin> Vins { get; set; }

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
        public List<Script> Scripts { get; set; }

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
        public List<Claim> Claims { get; set; }

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("block_hash")]
        public string BlockHash { get; set; }

        [JsonProperty("attributes")]
        public List<object> Attributes { get; set; }

        [JsonProperty("asset_moved")]
        public string AssetMoved { get; set; }

        [JsonProperty("asset")]
        public object Asset { get; set; }

        public static Transaction FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Transaction>(json);
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


//{
//"vouts": [
//{
//    "value": 0.01171044,
//    "n": 0,
//    "asset": "GAS",
//    "address": "AQ4TBSsURiEmRTDzfc9EGfrp385L5oGX5Y"
//}
//],
//"vin": [],
//"version": 0,
//"type": "ClaimTransaction",
//"txid": "1057eee0e5a21ca612bc2d274842dc4f3cb4f7a5e4342a9296f4ad52e0a0a38b",
//"time": 1523456312,
//"sys_fee": "0",
//"size": 203,
//"scripts": [
//{
//    "verification": "2102d637fe551008db2d66609b9929264301f67f50f778f2dfada5f36e75758aa6efac",
//    "invocation": "408261e48cfb30f5d953172b276ad9b0fb33eae18edc5fdc7895c4ffffd387fb7473cc6a4ac4c225d58cc19f4d12b87304e3bae311cce5eb1ca1413fd7c4c7a280"
//}
//],
//"script": null,
//"pubkey": null,
//"nonce": null,
//"net_fee": "0",
//"descriptors": null,
//"description": null,
//"contract": null,
//"claims": [
//{
//    "value": 18,
//    "txid": "24db8e3557277220c4bbdbaa7ab385921a02a90ab5a6dd915ce5d39e5d6fff40",
//    "n": 0,
//    "asset": "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b",
//    "address_hash": "AQ4TBSsURiEmRTDzfc9EGfrp385L5oGX5Y"
//}
//],
//"block_height": 2133065,
//"block_hash": "27a022e66691fc40d264ef615cd1299c9247814fde451390c602160ae954881b",
//"attributes": [],
//"asset_moved": "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7",
//"asset": null
//}