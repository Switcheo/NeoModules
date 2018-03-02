using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
	public class Unspent
	{
		[JsonProperty("value")]
		public double Value { get; set; }

		[JsonProperty("txid")]
		public string Txid { get; set; }

		[JsonProperty("n")]
		public int N { get; set; }
	}
}
