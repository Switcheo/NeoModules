using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class AssetState
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public List<Name> Name { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("available")]
        public string Available { get; set; }

        [JsonProperty("precision")]
        public int Precision { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("admin")]
        public string Admin { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("expiration")]
        public uint Expiration { get; set; }

        [JsonProperty("frozen")]
        public bool Frozen { get; set; }
    }

    public class Name
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("name")]
        public string AssetName { get; set; }
    }
}


