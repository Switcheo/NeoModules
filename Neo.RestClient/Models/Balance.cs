using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RestClient.Models
{
    public class Balance
    {
        [JsonProperty("unspent")]
        public IList<Unspent> Unspent { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
