using Newtonsoft.Json;
using System.Collections.Generic;

namespace NeoModules.Rest.Models
{
	public class Claimed
	{
		public Claimed(string address, IList<ClaimedElement> claimedList)
		{
			Address = address;
			ClaimedList = claimedList;
		}

		[JsonProperty("claimed")]
		public IList<ClaimedElement> ClaimedList { get; set; }

		[JsonProperty("address")]
		public string Address { get; set; }

		public static Claimed FromJson(string json)
		{
			return JsonConvert.DeserializeObject<Claimed>(json);
		}
	}
}
