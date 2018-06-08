using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace NeoModules.RPC.Infrastructure
{
    public static class Nep5Helper
    {
        public static string HexToString(this string inputText)
        {
            var hex = Enumerable.Range(0, inputText.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(inputText.Substring(x, 2), 16))
                .ToArray();
            return Encoding.ASCII.GetString(hex);
        }

        public static BigInteger DecimalToBigInteger(int decimals)
        {
            return BigInteger.Parse(Math.Pow(10, Convert.ToDouble(decimals)).ToString(CultureInfo.InvariantCulture));
        }

        public static byte[] HexStringToBytes(this string value) //different name because of ambiguous call
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

        public static decimal CalculateDecimalFromBigInteger(BigInteger value, int decimals)
        {
            var balance = BigInteger.DivRem(value, DecimalToBigInteger(decimals), out var remainder);

            var balanceResult = balance.ToString();
            var balanceRemainder = remainder.ToString();
            var finalbalance = $"{balanceResult}.{balanceRemainder}";
            var testfinal = decimal.Parse(finalbalance, CultureInfo.InvariantCulture);
            return testfinal;
        }
    }
}