using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class TransactionHistory
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("history")]
        public List<History> History { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("net")]
        public string Net { get; set; }
    }
}