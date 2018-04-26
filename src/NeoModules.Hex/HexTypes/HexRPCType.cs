using NeoModules.Hex.HexConverters;
using NeoModules.Hex.HexConverters.Extensions;

namespace NeoModules.Hex.HexTypes
{
    public class HexRPCType<T>
    {
        protected IHexConverter<T> Converter;

        protected string hexValue;

        protected T value;

        protected HexRPCType(IHexConverter<T> converter)
        {
            Converter = converter;
        }

        public HexRPCType(IHexConverter<T> converter, string hexValue)
        {
            Converter = converter;
            InitialiseFromHex(hexValue);
        }

        public HexRPCType(T value, IHexConverter<T> converter)
        {
            Converter = converter;
            InitialiseFromValue(value);
        }

        public string HexValue
        {
            get => hexValue;
            set => InitialiseFromHex(hexValue);
        }

        public T Value
        {
            get => value;
            set => InitialiseFromValue(value);
        }

        protected void InitialiseFromHex(string newHexValue)
        {
            value = ConvertFromHex(newHexValue);
            hexValue = newHexValue.EnsureHexPrefix();
        }

        protected void InitialiseFromValue(T newValue)
        {
            hexValue = ConvertToHex(newValue).EnsureHexPrefix();
            value = newValue;
        }

        protected string ConvertToHex(T newValue)
        {
            return Converter.ConvertToHex(newValue);
        }

        protected T ConvertFromHex(string newHexValue)
        {
            return Converter.ConvertFromHex(newHexValue);
        }

        public byte[] ToHexByteArray()
        {
            return HexValue.HexToByteArray();
        }

        public static implicit operator byte[](HexRPCType<T> hexRpcType)
        {
            return hexRpcType.ToHexByteArray();
        }

        public static implicit operator T(HexRPCType<T> hexRpcType)
        {
            return hexRpcType.Value;
        }
    }
}