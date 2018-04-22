using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class FeesInRange
    {
        [JsonProperty("total_sys_fee")]
        public long TotalSysFee { get; set; }

        [JsonProperty("total_net_fee")]
        public long TotalNetFee { get; set; }

        public static FeesInRange FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<FeesInRange>(json, settings);
        }
    }
}
