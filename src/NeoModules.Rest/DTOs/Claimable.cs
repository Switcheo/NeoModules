using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class Claimable
    {
        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("claimable")]
        public IList<ClaimableElement> ClaimableList { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static Claimable FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<Claimable>(json, settings);
        }
    }

    public class ClaimableElement
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("unclaimed")]
        public float Unclaimed { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("start_height")]
        public int StartHeight { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("end_height")]
        public int EndHeight { get; set; }
    }
}