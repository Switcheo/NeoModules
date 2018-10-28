using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class ExecuteOrder
    {
        [JsonProperty("fills")]
        public Dictionary<string, string> Fills { get; set; } = new Dictionary<string, string>(); //  <fill_id_1>: <signature_1>,

        [JsonProperty("makes")]
        public Dictionary<string, string> Makes { get; set; } = new Dictionary<string, string>(); // <make_id>: <signature>
    }
}
