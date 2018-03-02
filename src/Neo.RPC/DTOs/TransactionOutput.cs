using Newtonsoft.Json;

namespace Neo.RPC.DTOs
{
    public class TransactionOutput
    {
		[JsonProperty("n")]
		public int N { get; set; }

		[JsonProperty("asset")]
		public string Asset { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("address")]
		public string Address { get; set; }
	}
}
