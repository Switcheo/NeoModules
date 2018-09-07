using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class SimpleNode
    {
        [JsonProperty("address_id")]
        public long AddressId { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("protocol")]
        public Protocol Protocol { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("address")]
        public Uri Address { get; set; }

        public static IList<SimpleNode> FromJson(string json) => JsonConvert.DeserializeObject<IList<SimpleNode>>(json, Utils.Settings);
    }
}
