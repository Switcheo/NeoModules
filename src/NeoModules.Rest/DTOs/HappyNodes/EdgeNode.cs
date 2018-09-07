using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class EdgeNode
    {
        [JsonProperty("source_address_id")]
        public long SourceAddressId { get; set; }

        [JsonProperty("source_address")]
        public Uri SourceAddress { get; set; }

        [JsonProperty("validated_peers_address_id")]
        public long ValidatedPeersAddressId { get; set; }

        [JsonProperty("validated_peers_address")]
        public Uri ValidatedPeersAddress { get; set; }

        public static IList<EdgeNode> FromJson(string json) => JsonConvert.DeserializeObject<IList<EdgeNode>>(json, Utils.Settings);
    }
}
