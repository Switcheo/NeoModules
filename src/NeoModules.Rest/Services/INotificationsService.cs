using System.Threading.Tasks;
using NeoModules.Rest.DTOs.NeoNotifications;

namespace NeoModules.Rest.Services
{
    public interface INotificationsService
    {
        Task<ContractResult> GetContractNotifications(string scriptHash, int page = 1,
            string eventType = "", int afterBlock = 0, int pageSize = 100);

        Task<TokenResult> GetTokens(int page = 1, string eventType = "", int afterBlock = 0,
            int pageSize = 100);

        Task<BlockResult> GetBlockNotifications(int blockHeight, int page = 1, string eventType = "",
            int afterBlock = 0, int pageSize = 100);

        Task<AddressResult> GetAddressNotifications(string address, int page = 1, string eventType = "",
            int afterBlock = 0, int pageSize = 100);
    }
}