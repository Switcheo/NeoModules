using NeoModules.RPC.TransactionManagers;

namespace NeoModules.RPC.Infrastructure
{
    public interface IAccount
    {
        string Address { get; }
        ITransactionManager TransactionManager { get; }
    }
}