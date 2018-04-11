using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class Node
    {
        [JsonConstructor]
        public Node(string url, long height)
        {
            Url = url;
            Height = height;
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        public static List<Node> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<Node>>(json);
        }
    }
}