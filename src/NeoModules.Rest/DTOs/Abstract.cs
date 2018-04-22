using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class AbstractAddress
    {
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_entries")]
        public int TotalEntries { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        [JsonProperty("entries")]
        public IList<AbstractEntry> Entries { get; set; }

        public static AbstractAddress FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<AbstractAddress>(json, settings);
        }
    }
}

