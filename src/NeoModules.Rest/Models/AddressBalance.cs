using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class AddressBalance
    {
        [JsonConstructor]
        public AddressBalance(IList<Balance> balance, string address)
        {
            Balance = balance;
            Address = address;
        }

        [JsonProperty("balance")]
        public IList<Balance> Balance { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static AddressBalance FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AddressBalance>(json);
        }
    }
}