using System.Collections.Generic;
using Newtonsoft.Json;

namespace Neo.RestClient.Models
{
    public class AddressBalance
    {
        [JsonProperty("balance")]
        public IList<Balance> Balance { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static AddressBalance FromJson(string json) => JsonConvert.DeserializeObject<AddressBalance>(json);
    }
}
