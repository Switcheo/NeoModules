using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class AddressBalance
    {
        [JsonConstructor]
        public AddressBalance(IList<Balance> balance, string address)
        {
            Balance = balance;
            Address = address;
        }

        [JsonProperty("balance")]
        public IList<Balance> Balance { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static AddressBalance FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<AddressBalance>(json,settings);
        }
    }

    public class Balance
    {
        [JsonConstructor]
        public Balance(IList<Unspent> unspentList, string asset, float amount)
        {
            Unspent = unspentList;
            Asset = asset;
            Amount = amount;
        }

        [JsonProperty("unspent")]
        public IList<Unspent> Unspent { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }
    }

    public class Unspent
    {
        [JsonConstructor]
        public Unspent(float value, string txId, int n)
        {
            Value = value;
            TxId = txId;
            N = n;
        }

        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }
    }
}