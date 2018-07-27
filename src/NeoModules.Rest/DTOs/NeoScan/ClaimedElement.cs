using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.NeoScan
{
    public class ClaimedElement
    {
        [JsonProperty("txids")]
        public IList<string> Txids { get; set; }
    }
}
