using Newtonsoft.Json;
using System.Collections.Generic;

namespace NeoModules.Rest.Models
{
	public class ClaimedElement
	{
		[JsonProperty("txids")]
		public List<string> Txids { get; set; }
	}
}
