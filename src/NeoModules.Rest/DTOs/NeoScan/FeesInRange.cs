using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoScan
{
    public class FeesInRange
    {
        [JsonProperty("total_sys_fee")]
        public long TotalSysFee { get; set; }

        [JsonProperty("total_net_fee")]
        public long TotalNetFee { get; set; }

        public static FeesInRange FromJson(string json) => JsonConvert.DeserializeObject<FeesInRange>(json, Utils.Settings);
    }
}
