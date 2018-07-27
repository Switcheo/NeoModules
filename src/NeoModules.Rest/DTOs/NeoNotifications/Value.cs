using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoNotifications
{
    public class Value
    {
        [JsonProperty("type")]
        public StateType Type { get; set; }

        [JsonProperty("value")]
        public string ValueValue { get; set; }
    }
}
