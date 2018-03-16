using Newtonsoft.Json;

namespace NeoModules.RPC.DTOs
{
	public class InvokeParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}
}
