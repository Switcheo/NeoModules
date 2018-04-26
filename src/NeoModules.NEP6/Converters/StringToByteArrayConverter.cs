using System;
using NeoModules.Core;
using Newtonsoft.Json;

namespace NeoModules.NEP6.Converters
{
    public class StringToByteArrayConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            byte[] data = (byte[])value;
            var hexString = data.ToHexString();

            writer.WriteValue(hexString);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string hex = (string)reader.Value;
            var result = hex.HexToBytes();
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
