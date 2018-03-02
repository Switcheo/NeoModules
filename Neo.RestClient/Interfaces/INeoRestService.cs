using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Neo.RestClient.Interfaces
{
    public interface INeoRestService
    {
        Task<string> GetBalanceAsync(string address);
    }
}