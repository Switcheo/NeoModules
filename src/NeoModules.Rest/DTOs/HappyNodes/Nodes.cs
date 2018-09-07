using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class Nodes
    {
        [JsonProperty("online")]
        public NodesByRegion Online { get; set; }

        [JsonProperty("offline")]
        public NodesByRegion Offline { get; set; }

        public static Nodes FromJson(string json) => JsonConvert.DeserializeObject<Nodes>(json, Utils.Settings);
    }

    public class NodesByRegion
    {
        [JsonProperty("asia")]
        public IList<FlatNode> Asia { get; set; }

        [JsonProperty("europe")]
        public IList<FlatNode> Europe { get; set; }

        [JsonProperty("americas")]
        public IList<FlatNode> Americas { get; set; }

        [JsonProperty("africa")]
        public IList<FlatNode> Africa { get; set; }

        [JsonProperty("oceania")]
        public IList<FlatNode> Oceania { get; set; }
    }
}
