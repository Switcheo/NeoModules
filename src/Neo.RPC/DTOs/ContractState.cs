using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class ContractState
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("parameters")]
        public List<string> Parameters { get; set; }

        [JsonProperty("returntype")]
        public string ReturnType { get; set; }

        [JsonProperty("storage")]
        public bool Storage { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code_version")]
        public string CodeVersion { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
