using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.TransactionManagers
{
    public class TransactionManager : TransactionManagerBase
    {
        public TransactionManager(IClient client)
        {
            Client = client;
        }
    }
}