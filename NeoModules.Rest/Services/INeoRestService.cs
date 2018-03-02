using System.Threading.Tasks;

namespace NeoModules.Rest.Services
{
	public interface INeoRestService
	{
		Task<string> GetBalanceAsync(string address);
	}
}
