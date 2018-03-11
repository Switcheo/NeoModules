using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.RPC.DTOs
{
    public class ApplicationLog
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("vmstate")]
        public string VmState { get; set; }

        [JsonProperty("gas_consumed")]
        public string GasConsumed { get; set; }

        [JsonProperty("stack")]
        public List<object> Stack { get; set; } // todo need to see examples of this

        [JsonProperty("notifications")]
        public List<Notification> Notifications { get; set; }
    }

    public class Notification
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }

        [JsonProperty("state")]
        public List<Stack> State { get; set; }
    }
}
