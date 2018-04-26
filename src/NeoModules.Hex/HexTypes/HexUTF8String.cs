using NeoModules.Hex.HexConverters;
using Newtonsoft.Json;

namespace NeoModules.Hex.HexTypes
{
    [JsonConverter(typeof(HexRPCTypeJsonConverter<HexUTF8String, string>))]
    public class HexUTF8String : HexRPCType<string>
    {
        private HexUTF8String() : base(new HexUTF8StringConverter())
        {
        }

        public HexUTF8String(string value) : base(value, new HexUTF8StringConverter())
        {
        }

        public static HexUTF8String CreateFromHex(string hex)
        {
            return new HexUTF8String {HexValue = hex};
        }
    }
}