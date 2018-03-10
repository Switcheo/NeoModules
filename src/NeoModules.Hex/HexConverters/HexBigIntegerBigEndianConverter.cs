using System.Numerics;
using NeoModules.Hex.HexConverters.Extensions;

namespace NeoModules.Hex.HexConverters
{
    public class HexBigIntegerBigEndianConverter : IHexConverter<BigInteger>
    {
        public string ConvertToHex(BigInteger newValue)
        {
            return newValue.ToHex(false);
        }

        public BigInteger ConvertFromHex(string hex)
        {
            return hex.HexToBigInteger(false);
        }
    }
}
