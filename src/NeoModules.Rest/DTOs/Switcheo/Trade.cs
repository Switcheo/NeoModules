using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class Trade
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("fill_amount")]
        public long FillAmount { get; set; }

        [JsonProperty("take_amount")]
        public long TakeAmount { get; set; }

        [JsonProperty("event_time")]
        public DateTimeOffset EventTime { get; set; }

        [JsonProperty("is_buy")]
        public bool IsBuy { get; set; }

        public static List<Trade> FromJson(string json) => JsonConvert.DeserializeObject<List<Trade>>(json, Utils.Settings);
    }
}
