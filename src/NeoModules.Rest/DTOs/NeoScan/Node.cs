using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoScan
{
    public class Node
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        public static IList<Node> FromJson(string json) => JsonConvert.DeserializeObject<IList<Node>>(json, Utils.Settings);
    }
}