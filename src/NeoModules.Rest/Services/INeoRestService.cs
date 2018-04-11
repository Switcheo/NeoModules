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
    }
}
