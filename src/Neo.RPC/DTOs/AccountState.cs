using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class AccountState
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("script_hash")]
        public string ScriptHash { get; set; }

        [JsonProperty("frozen")]
        public bool Frozen { get; set; }

        [JsonProperty("votes")]
        public List<object> Votes { get; set; }

        [JsonProperty("balances")]
        public List<AccountBalance> Balance { get; set; }
    }

    public class AccountBalance
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
