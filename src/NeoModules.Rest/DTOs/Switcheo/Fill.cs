using System;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class Fill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("offer_hash")]
        public string OfferHash { get; set; }

        [JsonProperty("offer_asset_id")]
        public string OfferAssetId { get; set; }

        [JsonProperty("want_asset_id")]
        public string WantAssetId { get; set; }

        [JsonProperty("fill_amount")]
        public string FillAmount { get; set; }

        [JsonProperty("want_amount")]
        public string WantAmount { get; set; }

        [JsonProperty("filled_amount")]
        public string FilledAmount { get; set; }

        [JsonProperty("fee_asset_id")]
        public string FeeAssetId { get; set; }

        [JsonProperty("fee_amount")]
        public string FeeAmount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("txn")]
        public Txn Txn { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("transaction_hash")]
        public string TransactionHash { get; set; }
    }
}
