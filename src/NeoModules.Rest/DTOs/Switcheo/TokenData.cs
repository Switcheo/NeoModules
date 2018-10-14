using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class TokenData
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }

        public static Dictionary<string, TokenData> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, TokenData>>(json, Utils.Settings);
    }
}
