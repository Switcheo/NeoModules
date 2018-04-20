using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
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
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<Claimed>(json, settings);
        }
    }

    public class ClaimedElement
    {
        [JsonProperty("txids")]
        public List<string> Txids { get; set; }
    }
}
