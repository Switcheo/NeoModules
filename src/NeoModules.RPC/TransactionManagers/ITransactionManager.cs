using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.TransactionManagers
{
    public interface ITransactionManager
    {
        IClient Client { get; set; }
        Task<string> SendTransactionAsync(string from, string to, decimal amount);
        Task<bool> SendRawTransactionAsync(string hexTx);
    }
}