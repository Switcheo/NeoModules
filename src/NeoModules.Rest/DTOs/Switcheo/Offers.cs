using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class Offers
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("offer_asset")]
        public string OfferAsset { get; set; }

        [JsonProperty("want_asset")]
        public string WantAsset { get; set; }

        [JsonProperty("available_amount")]
        public long AvailableAmount { get; set; }

        [JsonProperty("offer_amount")]
        public long OfferAmount { get; set; }

        [JsonProperty("want_amount")]
        public long WantAmount { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public static List<Offers> FromJson(string json) => JsonConvert.DeserializeObject<List<Offers>>(json, Utils.Settings);
    }
}
