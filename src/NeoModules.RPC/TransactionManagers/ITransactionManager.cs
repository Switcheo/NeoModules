using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.TransactionManagers
{
    public interface ITransactionManager
    {
        IClient Client { get; set; }
        Task<bool> SendTransactionAsync(string hexTx);
    }
}