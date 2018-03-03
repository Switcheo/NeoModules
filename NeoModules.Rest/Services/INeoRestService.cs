using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NeoModules.Rest.Services
{
	public interface INeoRestService
	{
		Task<string> GetBalanceAsync(string address);
	    Task<string> GetClaimable(string address);
	    Task<string> GetClaimed(string address);
    }
}
