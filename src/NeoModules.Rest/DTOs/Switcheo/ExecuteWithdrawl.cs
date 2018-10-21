using System;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class ExecuteWithdrawl
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
