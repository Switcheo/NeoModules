using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class EndPoints
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pollTime")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long PollTime { get; set; }

        [JsonProperty("sites")]
        public List<Site> Sites { get; set; }

        public static EndPoints FromJson(string json) => JsonConvert.DeserializeObject<EndPoints>(json, Utils.Settings);
    }

    public class Site
    {
        [JsonProperty("protocol")]
        public Protocol Protocol { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

}
