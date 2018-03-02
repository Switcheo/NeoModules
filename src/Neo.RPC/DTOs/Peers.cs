using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo.RPC.DTOs
{
    public class Peers
    {
		[JsonProperty("bad")]
		public List<Peer> Bad { get; set; }

		[JsonProperty("connected")]
		public List<Peer> Connected { get; set; }

		[JsonProperty("unconnected")]
		public List<Peer> Unconnected { get; set; }
	}
}
