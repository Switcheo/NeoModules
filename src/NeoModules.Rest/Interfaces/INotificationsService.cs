using System.Threading.Tasks;
using NeoModules.Rest.DTOs.NeoNotifications;

namespace NeoModules.Rest.Interfaces
{
    public interface INotificationsService
    {
        Task<ContractResult> GetContractNotifications(string scriptHash, int page = -1,
            string eventType = "", int afterBlock = -1, int beforeBlock = -1, int pageSize = -1);

        Task<TokenResult> GetTokens(int page = -1, string eventType = "", int afterBlock = -1, int beforeBlock = -1,
            int pageSize = -1);

        Task<BlockResult> GetBlockNotifications(int blockHeight, int page = -1, string eventType = "",
            int afterBlock = -1, int beforeBlock = -1, int pageSize = -1);

        Task<AddressResult> GetAddressNotifications(string address, int page = -1, string eventType = "",
            int afterBlock = -1, int beforeBlock = -1, int pageSize = -1);

        Task<TransactionResult> GetTransactionNotifications(string txHash, int page = -1, string eventType = "",
            int afterBlock = -1, int beforeBlock = -1, int pageSize = -1);
    }
}