namespace NeoModules.Hex.HexConverters
{
    public interface IHexConverter<T>
    {
        string ConvertToHex(T value);
        T ConvertFromHex(string value);
    }
}
