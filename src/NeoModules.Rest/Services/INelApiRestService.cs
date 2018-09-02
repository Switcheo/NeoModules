using System.Threading.Tasks;

namespace NeoModules.Rest.Services
{
    public interface INelApiRestService //todo: DTOs
    {
        Task<long> GetBlockCount(); //GetHeight
        Task<string> GetBlock(int height);
        Task<string> GetRawTransaction(string txHash);
        Task<string> GetAsset(string assetHash);
        Task<string> GetFullLog(string txHash);
        Task<string> GetNotify(string txHash);
        Task<string> GetUtxo(string address);
        Task<string> GetUtxoCount(string address);
        Task<string> GetUtxosToPay(string address, string txHash, decimal firstValue, decimal secondValue);
        Task<string> GetBalance(string address);
        Task<string> GetBlocks(int maxHeigth, int minHeight);
        Task<string>
            GetRawTransactions(int maxHeight, int minHeight,
                string type); //txType:EnrollmentTransaction,PublishTransaction,InvocationTransaction,ClaimTransaction,MinerTransaction,RegisterTransaction,IssueTransaction,ContractTransaction
    }
}