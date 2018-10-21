using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class RecentTrade
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("pair")]
        public string Pair { get; set; }

        [JsonProperty("side")]
        public Side Side { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        public static List<RecentTrade> FromJson(string json) =>
            JsonConvert.DeserializeObject<List<RecentTrade>>(json, Utils.Settings);
    }

    public enum Side { Buy, Sell };
}
