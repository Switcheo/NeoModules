using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NeoModules.RPC.Helpers
{
    public static class Helper
    {
        public static byte[] HexToBytes(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new byte[0];
            if (value.Length % 2 == 1)
                throw new FormatException();
            var result = new byte[value.Length / 2];
            for (var i = 0; i < result.Length; i++)
                result[i] = byte.Parse(value.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
            return result;
        }

        public static string HexToString(this string inputText)
        {
            var bb = Enumerable.Range(0, inputText.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(inputText.Substring(x, 2), 16))
                .ToArray();
            return Encoding.ASCII.GetString(bb);
        }
    }
}