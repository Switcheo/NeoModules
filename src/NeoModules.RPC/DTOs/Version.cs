using Newtonsoft.Json;

namespace NeoModules.RPC.DTOs
{
    public class Version
    {
		[JsonProperty("port")]
		public int Port { get; set; }

		[JsonProperty("nonce")]
		public int Nonce { get; set; }

		[JsonProperty("useragent")]
		public string UserAgent { get; set; }
	}
}
