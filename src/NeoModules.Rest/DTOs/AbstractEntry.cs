using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class AbstractEntry
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("address_to")]
        public string AddressTo { get; set; }

        [JsonProperty("address_from")]
        public string AddressFrom { get; set; }
    }
}
