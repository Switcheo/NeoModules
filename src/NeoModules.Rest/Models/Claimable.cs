using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class Claimable
    {
        [JsonConstructor]
        public Claimable(float unclaimed, IList<ClaimableElement> claimableList, string address)
        {
            Unclaimed = unclaimed;
			ClaimableList = claimableList;
            Address = address;
        }

        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("claimable")]
        public IList<ClaimableElement> ClaimableList { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static Claimable FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Claimable>(json);
        }
    }
}