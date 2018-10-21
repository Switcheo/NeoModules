using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.Switcheo
{
    public class OrderResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("blockchain")]
        public string Blockchain { get; set; }

        [JsonProperty("contract_hash")]
        public string ContractHash { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("offer_asset_id")]
        public string OfferAssetId { get; set; }

        [JsonProperty("want_asset_id")]
        public string WantAssetId { get; set; }

        [JsonProperty("offer_amount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long OfferAmount { get; set; }

        [JsonProperty("want_amount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long WantAmount { get; set; }

        [JsonProperty("transfer_amount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long TransferAmount { get; set; }

        [JsonProperty("priority_gas_amount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long PriorityGasAmount { get; set; }

        [JsonProperty("use_native_token")]
        public bool UseNativeToken { get; set; }

        [JsonProperty("native_fee_transfer_amount")]
        public long NativeFeeTransferAmount { get; set; }

        [JsonProperty("deposit_txn")]
        public object DepositTxn { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("order_status")]
        public string OrderStatus { get; set; }

        [JsonProperty("fills")]
        public List<Fill> Fills { get; set; }

        [JsonProperty("makes")]
        public List<Make> Makes { get; set; }

        public static OrderResponse FromJson(string json) =>
            JsonConvert.DeserializeObject<OrderResponse>(json, Utils.Settings);
    }
}
