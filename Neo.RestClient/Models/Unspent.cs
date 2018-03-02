using Newtonsoft.Json;

namespace Neo.RestClient.Models
{
    public class Unspent
    {
        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }
    }
}
