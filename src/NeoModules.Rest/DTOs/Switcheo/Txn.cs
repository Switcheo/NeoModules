using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class Txn
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("invoke")]
        public Invoke Invoke { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }

        [JsonProperty("inputs")]
        public List<Input> Inputs { get; set; }

        [JsonProperty("outputs")]
        public List<Output> Outputs { get; set; }

        [JsonProperty("scripts")]
        public List<object> Scripts { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("gas")]
        public decimal Gas { get; set; }
    }

    public class Attribute
    {
        [JsonProperty("usage")]
        public int Usage { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public class Input
    {
        [JsonProperty("prevHash")]
        public string PrevHash { get; set; }

        [JsonProperty("prevIndex")]
        public int PrevIndex { get; set; }
    }

    public class Invoke
    {
        [JsonProperty("scriptHash")]
        public string ScriptHash { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("args")]
        public List<object> Args { get; set; }
    }

    public class Output
    {
        [JsonProperty("assetId")]
        public string AssetId { get; set; }

        [JsonProperty("scriptHash")]
        public string ScriptHash { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
