using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class Claimable
    {
        [JsonConstructor]
        public Claimable(float unclaimed, ClaimableElement[] claimableElements, string address)
        {
            Unclaimed = unclaimed;
            ClaimableClaimable = claimableElements;
            Address = address;
        }

        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("claimable")]
        public ClaimableElement[] ClaimableClaimable { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static Claimable FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Claimable>(json);
        }
    }
}