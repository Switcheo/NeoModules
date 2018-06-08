using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.TransactionManagers
{
    public class TransactionManager : TransactionManagerBase
    {
        public TransactionManager(IClient client)
        {
            Client = client;
        }

        public override Task<string> SignTransactionAsync(byte[] transactionData)
        {
            throw new System.NotImplementedException();
        }
    }
}