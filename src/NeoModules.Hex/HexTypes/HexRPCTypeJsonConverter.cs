using System;
using Newtonsoft.Json;

namespace NeoModules.Hex.HexTypes
{
    public class HexRPCTypeJsonConverter<T, TValue> : JsonConverter where T : HexRPCType<TValue>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var hexRpcType = (T) value;
            writer.WriteValue(hexRpcType.HexValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return HexTypeFactory.CreateFromHex<TValue>((string) reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}