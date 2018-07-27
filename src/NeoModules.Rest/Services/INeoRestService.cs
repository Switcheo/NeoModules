using System.Threading.Tasks;

namespace NeoModules.Rest.Services
{
    public interface INeoRestService
    {
        Task<string> GetBalanceAsync(string address);
        Task<string> GetClaimableAsync(string address);
        Task<string> GetClaimedAsync(string address);
        Task<string> GetUnclaimedAsync(string address);
        Task<string> GetAddressAsync(string address);
        Task<string> GetTransactionAsync(string hash);
        Task<string> GetAllNodesAsync();
        Task<string> GetAssetsAsync();
        Task<string> GetAssetAsync(string assetHash);
        Task<string> GetHeight();
        Task<string> GetHighestBlock();
        Task<string> GetLastBlocks();
        Task<string> GetFeesInRange(int range1, int range2);
        Task<string> GetAddressAbstracts(string address, int page);
        Task<string> GetNeonAddress(string address);
        Task<string> GetAddressToAddressAbstract(string addressFrom, string addressTo,int page);
        Task<string> GetBlock(string blockHash);
        Task<string> GetBlock(int blockHeight);
        Task<string> GetLastTransactions(string type = null);
        Task<string> GetLastTransactionsByAddress(string address, int page);
        Task<string> GetNodes();
    }
}