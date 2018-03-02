using NeoModules.RPC.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NeoModules.RPC.DTOs
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
