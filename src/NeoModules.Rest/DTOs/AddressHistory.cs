using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class AddressHistory
    {
        [JsonConstructor]
        public AddressHistory(float unclaimed, IList<TxId> txids, int txCount, long time, IList<ClaimedElement> claimed,
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

        public static AddressHistory FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<AddressHistory>(json, settings);
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
        public IList<TxidBalance> Balance { get; set; }
    }

    public class TxidBalance
    {
        [JsonProperty("asset")]
        public GovernanceAsset Asset { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }
    }

    public enum GovernanceAsset
    {
        Gas,
        Neo
    }
}