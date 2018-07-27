using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class State
    {
        [JsonProperty("type")]
        public StateType Type { get; set; }

        [JsonProperty("value")]
        public List<Value> Value { get; set; }
    }
}
