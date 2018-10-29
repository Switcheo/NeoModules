using System.Collections.Generic;
using NeoModules.Rest.Helpers;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class HistoricNetworkSize
    {
        [JsonProperty("data")]
        public List<NodeNetworkHistory> Data { get; set; }

        public static HistoricNetworkSize FromJson(string json) => JsonConvert.DeserializeObject<HistoricNetworkSize>(json, Utils.Settings);
    }

    public class NodeNetworkHistory
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("totalonline")]
        public long Totalonline { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
}
