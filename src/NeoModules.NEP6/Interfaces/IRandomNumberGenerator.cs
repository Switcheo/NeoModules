namespace NeoModules.NEP6.Interfaces
{
    public interface IRandomNumberGenerator
    {
        byte[] GenerateNonce(int size);
    }
}
