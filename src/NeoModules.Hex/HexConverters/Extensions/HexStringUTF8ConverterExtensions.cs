using System.Text;

namespace NeoModules.Hex.HexConverters.Extensions
{
    public static class HexStringUTF8ConverterExtensions
    {
        public static string ToHexUTF8(this string value)
        {
            return "0x" + Encoding.UTF8.GetBytes(value).ToHex();
        }

        public static string HexToUTF8String(this string hex)
        {
            var bytes = hex.HexToByteArray();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}