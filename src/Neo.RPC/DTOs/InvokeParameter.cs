using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neo.RPC.DTOs
{
	public class InvokeParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}
}
