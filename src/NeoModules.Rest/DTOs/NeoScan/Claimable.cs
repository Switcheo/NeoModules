using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoScan
{
    public class Claimable
    {
        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("claimable")]
        public IList<ClaimableElement> ClaimableList { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static Claimable FromJson(string json) => JsonConvert.DeserializeObject<Claimable>(json, Utils.Settings);
    }

    public class ClaimableElement
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("sys_fee")]
        public double SysFee { get; set; }

        [JsonProperty("start_height")]
        public int StartHeight { get; set; }

        [JsonProperty("n")]
        public uint N { get; set; }

        [JsonProperty("generated")]
        public double Generated { get; set; }

        [JsonProperty("end_height")]
        public long EndHeight { get; set; }
    }
}