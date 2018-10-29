using System.Collections.Generic;
using NeoModules.Rest.Helpers;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class CandleStick
    {
        [JsonProperty("time")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Time { get; set; }

        [JsonProperty("pair")]
        public string Pair { get; set; }

        [JsonProperty("open")]
        public string Open { get; set; }

        [JsonProperty("close")]
        public string Close { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("quote_volume")]
        public string QuoteVolume { get; set; }

        public static List<CandleStick> FromJson(string json) => JsonConvert.DeserializeObject<List<CandleStick>>(json, Utils.Settings);
    }
}
