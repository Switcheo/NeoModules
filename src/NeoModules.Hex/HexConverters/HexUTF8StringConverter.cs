using System;
using NeoModules.Hex.HexConverters.Extensions;

namespace NeoModules.Hex.HexConverters
{
    public class HexUTF8StringConverter : IHexConverter<string>
    {
        public string ConvertToHex(String value)
        {
            return value.ToHexUTF8();
        }

        public String ConvertFromHex(string hex)
        {
            return hex.HexToUTF8String();
        }
    }
}
