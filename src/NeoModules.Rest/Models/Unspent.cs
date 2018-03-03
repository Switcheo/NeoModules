using Newtonsoft.Json;

namespace NeoModules.Rest.Models
{
    public class Unspent
    {
        [JsonConstructor]
        public Unspent(float value, string txId, int n)
        {
            Value = value;
            TxId = txId;
            N = n;
        }

        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }
    }
}