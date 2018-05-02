using Newtonsoft.Json;

namespace NeoModules.RPC.Infrastructure
{
    public class CallInput
    {
        public CallInput()
        {
        }

        public CallInput(string data, string addressTo, string symbol, decimal value)
        {
            Value = value;
            Symbol = symbol;
            Data = data;
            To = addressTo;
        }

        /// <summary>
        ///     DATA - The address the transaction is send from.
        /// </summary>
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        /// <summary>
        ///     DATA - The address the transaction is directed to.
        /// </summary>
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        /// <summary>
        ///     value: QUANTITY - (optional) Decimal of the value send with this transaction
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

        /// <summary>
        ///     data: DATA - (optional) The serialized (and signed opt) tx
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        /// <summary>
        ///     data: DATA - (optional) Symbol
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Symbol { get; set; }
    }
}