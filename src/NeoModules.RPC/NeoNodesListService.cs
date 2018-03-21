using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NeoModules.RPC
{
	// simple service, don't need to separate DTO's and the service does not belong in the same context as RPC
	public class NeoNodesListService
	{
		private static readonly string monitorCityOfZionTestNet = "http://monitor.cityofzion.io/assets/testnet.json";
		private static readonly string monitorCityOfZionMainNet = "http://monitor.cityofzion.io/assets/mainnet.json";

		public async Task<string> GetNodesList(MonitorNet net)
		{
			HttpClient _restClient = new HttpClient { };
			HttpResponseMessage resultMessage;
			if (net == MonitorNet.MainNet)
			{
				resultMessage = await _restClient?.GetAsync(monitorCityOfZionMainNet);
			}
			else
			{
				resultMessage = await _restClient?.GetAsync(monitorCityOfZionTestNet);
			}
			var data = await resultMessage.Content.ReadAsStringAsync();
			return data;
		}
	}

	public enum MonitorNet { MainNet, TestNet }
	public enum Protocol { Http, Https };
	public enum SiteType { Rest, Rpc };

	public class NodeList
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("pollTime")]
		public string PollTime { get; set; }

		[JsonProperty("sites")]
		public List<Site> Sites { get; set; }
	}

	public class Site
	{
		[JsonProperty("service")]
		public string Service { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("address")]
		public string Address { get; set; }

		[JsonProperty("locale")]
		public string Locale { get; set; }

		[JsonProperty("type")]
		public SiteType Type { get; set; }

		[JsonProperty("protocol")]
		public Protocol? Protocol { get; set; }

		[JsonProperty("port")]
		public string Port { get; set; }
	}
}
