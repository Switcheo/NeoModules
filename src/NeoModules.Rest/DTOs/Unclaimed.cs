using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class Unclaimed
    {
        [JsonConstructor]
        public Unclaimed(float unclaimedValue, string address)
        {
            UnclaimedValue = unclaimedValue;
            Address = address;
        }

        [JsonProperty("unclaimed")]
        public float UnclaimedValue { get; set; }

        public string Address { get; set; }

        public static Unclaimed FromJson(string json) => JsonConvert.DeserializeObject<Unclaimed>(json, Utils.Settings);
    }
}