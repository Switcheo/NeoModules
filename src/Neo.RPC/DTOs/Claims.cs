using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class Claims
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("claims")]
        public List<Claim> ClaimList { get; set; }

        [JsonProperty("net")]
        public string Net { get; set; }

        [JsonProperty("total_claim")]
        public int TotalClaim { get; set; }

        [JsonProperty("total_unspent_claim")]
        public int TotalUnspentClaim { get; set; }
    }
}
