using Neo.RPC.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo.RPC.DTOs
{
	public class Invoke
	{
		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("gas_consumed")]
		public string GasConsumed { get; set; }

		[JsonProperty("stack")]
		public List<Stack> Stack { get; set; }
	}
}
