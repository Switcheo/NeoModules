namespace NeoModules.NEP6.Interfaces
{
    public interface IRandomNumberGenerator
    {
        //byte[] GenerateRandomInitialisationVector();
        //byte[] GenerateRandomSalt();
        byte[] GenerateRandomBytes(int size);
    }
}
