using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class ExecuteDeposit
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
