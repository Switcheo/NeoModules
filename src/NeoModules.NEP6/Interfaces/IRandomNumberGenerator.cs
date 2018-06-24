namespace NeoModules.NEP6.Interfaces
{
    public interface IRandomNumberGenerator
    {
        byte[] GenerateRandomBytes(int size);
    }
}
