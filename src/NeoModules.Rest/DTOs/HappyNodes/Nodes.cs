using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class Nodes
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("protocol")]
        public Protocol Protocol { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static List<Nodes> FromJson(string json) => JsonConvert.DeserializeObject<List<Nodes>>(json, Utils.Settings);
    }
}
