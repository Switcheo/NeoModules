using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class Unconfirmed
    {
        [JsonProperty("txs")]
        public IList<UnconfirmedTransaction> UnconfirmedTransactions { get; set; }

        public static Unconfirmed FromJson(string json) => JsonConvert.DeserializeObject<Unconfirmed>(json, Utils.Settings);
    }

    public class UnconfirmedTransaction
    {
        [JsonProperty("connection_id")]
        public long ConnectionId { get; set; }

        [JsonProperty("protocol")]
        public Protocol Protocol { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("node_count")]
        public long NodeCount { get; set; }

        [JsonProperty("tx")]
        public string Tx { get; set; }

        [JsonProperty("last_blockheight")]
        public long LastBlockheight { get; set; }
    }
}
