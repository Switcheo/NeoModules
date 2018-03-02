using Newtonsoft.Json;
using System.Collections.Generic;

namespace NeoModules.Rest.Models
{
	public class Balance
	{
		[JsonProperty("unspent")]
		public IList<Unspent> Unspent { get; set; }

		[JsonProperty("asset")]
		public string Asset { get; set; }

		[JsonProperty("amount")]
		public double Amount { get; set; }
	}
}
