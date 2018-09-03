using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.Rest.DTOs.NeoScan;

namespace NeoModules.Rest.Interfaces
{
    public interface INeoscanService
    {
        Task<AddressBalance> GetBalanceAsync(string address);
        Task<Claimable> GetClaimableAsync(string address);
        Task<Claimed> GetClaimedAsync(string address);
        Task<Unclaimed> GetUnclaimedAsync(string address);
        Task<Transaction> GetTransactionAsync(string hash);
        Task<List<Node>> GetAllNodesAsync();
        Task<long> GetHeight();
        Task<AbstractAddress> GetAddressAbstracts(string address, int page);
        Task<AbstractAddress> GetAddressToAddressAbstract(string addressFrom, string addressTo,int page);
        Task<Block> GetBlock(string blockHash);
        Task<Block> GetBlock(int blockHeight);
        Task<List<Transaction>> GetLastTransactionsByAddress(string address, int page);
        Task<string> GetNodes();
    }
}