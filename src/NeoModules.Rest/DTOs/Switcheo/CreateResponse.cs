using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class CreateResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("transaction")]
        public Txn Transaction { get; set; }

        [JsonProperty("script_params")]
        public ScriptParams ScriptParams { get; set; }

        public static CreateResponse FromJson(string json) =>
            JsonConvert.DeserializeObject<CreateResponse>(json, Utils.Settings);
    }

    public class ScriptParams
    {
        [JsonProperty("scriptHash")]
        public string ScriptHash { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("args")]
        public List<object> Args { get; set; }
    }
}
