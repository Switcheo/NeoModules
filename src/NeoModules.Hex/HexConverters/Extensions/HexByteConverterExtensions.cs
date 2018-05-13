using System;
using System.Linq;

namespace NeoModules.Hex.HexConverters.Extensions
{
    public static class HexByteConverterExtensions
    {
        //From article http://blogs.msdn.com/b/heikkiri/archive/2012/07/17/hex-string-to-corresponding-byte-array.aspx

        private static readonly byte[] Empty = new byte[0];

        public static string ToHex(this byte[] value, bool prefix = false)
        {
            var strPrex = prefix ? "0x" : "";
            return strPrex + string.Concat(value.Select(b => b.ToString("x2")).ToArray());
        }

        public static bool HasHexPrefix(this string value)
        {
            return value.StartsWith("0x");
        }

        public static string RemoveHexPrefix(this string value)
        {
            return value.Replace("0x", "");
        }

        public static bool IsTheSameHex(this string first, string second)
        {
            return string.Equals(EnsureHexPrefix(first).ToLower(), EnsureHexPrefix(second).ToLower(),
                StringComparison.Ordinal);
        }

        public static string EnsureHexPrefix(this string value)
        {
            if (value == null) return null;
            if (!value.HasHexPrefix())
                return "0x" + value;
            return value;
        }

        public static string[] EnsureHexPrefix(this string[] values)
        {
            if (values != null)
                foreach (var value in values)
                    value.EnsureHexPrefix();
            return values;
        }

        public static string ToHexCompact(this byte[] value)
        {
            return ToHex(value).TrimStart('0');
        }

        public static byte[] HexToByteArray(this string value)
        {
            byte[] bytes;
            if (string.IsNullOrEmpty(value))
            {
                bytes = Empty;
            }
            else
            {
                var stringLength = value.Length;
                var characterIndex = value.StartsWith("0x", StringComparison.Ordinal) ? 2 : 0;
                // Does the string define leading HEX indicator '0x'. Adjust starting index accordingly.               
                var numberOfCharacters = stringLength - characterIndex;

                var addLeadingZero = false;
                if (0 != numberOfCharacters % 2)
                {
                    addLeadingZero = true;

                    numberOfCharacters += 1; // Leading '0' has been striped from the string presentation.
                }

                bytes = new byte[numberOfCharacters / 2]; // Initialize our byte array to hold the converted string.

                var writeIndex = 0;
                if (addLeadingZero)
                {
                    bytes[writeIndex++] = FromCharacterToByte(value[characterIndex], characterIndex);
                    characterIndex += 1;
                }

                for (var readIndex = characterIndex; readIndex < value.Length; readIndex += 2)
                {
                    var upper = FromCharacterToByte(value[readIndex], readIndex, 4);
                    var lower = FromCharacterToByte(value[readIndex + 1], readIndex + 1);

                    bytes[writeIndex++] = (byte) (upper | lower);
                }
            }

            return bytes;
        }

        private static byte FromCharacterToByte(char character, int index, int shift = 0)
        {
            var value = (byte) character;
            if (0x40 < value && 0x47 > value || 0x60 < value && 0x67 > value)
            {
                if (0x40 == (0x40 & value))
                    if (0x20 == (0x20 & value))
                        value = (byte) ((value + 0xA - 0x61) << shift);
                    else
                        value = (byte) ((value + 0xA - 0x41) << shift);
            }
            else if (0x29 < value && 0x40 > value)
            {
                value = (byte) ((value - 0x30) << shift);
            }
            else
            {
                throw new InvalidOperationException(string.Format(
                    "Character '{0}' at index '{1}' is not valid alphanumeric character.", character, index));
            }

            return value;
        }
    }
}