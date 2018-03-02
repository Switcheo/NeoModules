using Newtonsoft.Json;
using System.Collections.Generic;

namespace NeoModules.Rest.Models
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
