using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class AddressHistory
    {
        [JsonConstructor]
        public AddressHistory(float unclaimed, List<TxId> txids, int txCount, long time, List<Claimed> claimed,
            string address, List<Balance> balance)
        {
            Unclaimed = unclaimed;
            Txids = txids;
            TxCount = txCount;
            Time = time;
            Claimed = claimed;
            Address = address;
            Balance = balance;
        }

        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("txids")]
        public List<TxId> Txids { get; set; }

        [JsonProperty("tx_count")]
        public int TxCount { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("claimed")]
        public List<Claimed> Claimed { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("balance")]
        public List<Balance> Balance { get; set; }

        public static AddressHistory FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AddressHistory>(json);
        }
    }

    public class TxId
    {
        [JsonConstructor]
        public TxId(string txid, long blockHeight, List<TxidBalance> balance)
        {
            Txid = txid;
            BlockHeight = blockHeight;
            Balance = balance;
        }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("balance")]
        public List<TxidBalance> Balance { get; set; }
    }

    public class TxidBalance
    {
        [JsonProperty("asset")]
        public Asset Asset { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }
    }

    public class Claimed
    {
        [JsonProperty("txids")]
        public List<string> Txids { get; set; }
    }

    public enum Asset
    {
        Gas,
        Neo
    }
}