using NeoModules.NEP6.TransactionManagers;

namespace NeoModules.NEP6.Interfaces
{
    public interface IAccount
    {
        string Address { get; }
        ITransactionManager TransactionManager { get; }
        byte[] PrivateKey { get; }
    }
}