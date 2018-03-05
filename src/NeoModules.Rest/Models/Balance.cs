using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
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
}