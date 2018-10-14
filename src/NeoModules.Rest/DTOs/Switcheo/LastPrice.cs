using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class LastPrice
    {
        [JsonProperty("GAS")]
        public string GasPrice { get; set; }
        [JsonProperty("NEO")]
        public string NeoPrice { get; set; }
        [JsonProperty("SWTH")]
        public string SwthPrice { get; set; }

        public static Dictionary<string, LastPrice> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, LastPrice>>(json, Utils.Settings);

    }
}
