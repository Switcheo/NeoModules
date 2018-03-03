using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class ClaimableElement
    {
        [JsonConstructor]
        public ClaimableElement(float value, float unclaimed, string txid, int startHeight, int endHeight)
        {
            Value = value;
            Unclaimed = unclaimed;
            Txid = txid;
            StartHeight = startHeight;
            EndHeight = endHeight;
        }

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