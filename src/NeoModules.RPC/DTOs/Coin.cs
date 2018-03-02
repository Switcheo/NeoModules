using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.RPC.DTOs
{
    public class Coin
    {
        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("unspent")]
        public List<Unspent> Unspent { get; set; }
    }
}
