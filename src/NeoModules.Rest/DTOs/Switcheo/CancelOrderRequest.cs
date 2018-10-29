using System;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class CancelOrderRequest
    {
        [JsonProperty("order_id")]
        public Guid OrderId { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Timestamp { get; set; }
    }
}
