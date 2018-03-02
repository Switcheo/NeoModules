using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.RPC.DTOs
{
    public class Nodes
    {
        [JsonProperty("net")]
        public string Net { get; set; }

        [JsonProperty("nodes")]
        public List<Node> NodeList { get; set; }
    }
}