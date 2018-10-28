using System;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class Make
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("offer_hash")]
        public string OfferHash { get; set; }

        [JsonProperty("available_amount")]
        public string AvailableAmount { get; set; }

        [JsonProperty("offer_asset_id")]
        public string OfferAssetId { get; set; }

        [JsonProperty("offer_amount")]
        public string OfferAmount { get; set; }

        [JsonProperty("want_asset_id")]
        public string WantAssetId { get; set; }

        [JsonProperty("want_amount")]
        public string WantAmount { get; set; }

        [JsonProperty("filled_amount")]
        public string FilledAmount { get; set; }

        [JsonProperty("txn")]
        public Txn Txn { get; set; }

        [JsonProperty("cancel_txn")]
        public Txn CancelTxn { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("transaction_hash")]
        public string TransactionHash { get; set; }
    }
}
