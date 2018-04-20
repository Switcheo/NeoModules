using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.TransactionManagers
{
    public interface ITransactionManager
    {
        IClient Client { get; set; }
        Task<string> SendRawTransactionAsync(string from, string to, double amount);
        //Task<string> SendTransactionAsync(TransactionInput transactionInput);
    }
}