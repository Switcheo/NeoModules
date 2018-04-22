using System.Collections.Generic;
using Newtonsoft.Json;
namespace NeoModules.Rest.DTOs
{

    public class Asset
    {
        [JsonProperty("type")]
        public AssetType Type { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("precision")]
        public long Precision { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("name")]
        public IList<Name> Name { get; set; }

        [JsonProperty("issued")]
        public double? Issued { get; set; }

        [JsonProperty("contract")]
        public string Contract { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("admin")]
        public string Admin { get; set; }

        public static Asset FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<Asset>(json, settings);
        }
    }

    public class Assets
    {
        public static IList<Asset> FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<IList<Asset>>(json, settings);
        }
    }


    public class Name
    {
        [JsonProperty("name")]
        public string NameName { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }
    }

    public class Stats
    {
        [JsonProperty("transactions")]
        public long Transactions { get; set; }

        [JsonProperty("addresses")]
        public long Addresses { get; set; }
    }

    public enum AssetType { GoverningToken, Share, Token, UtilityToken };

}


