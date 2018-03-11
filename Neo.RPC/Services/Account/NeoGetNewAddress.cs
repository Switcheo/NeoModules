using System.Threading.Tasks;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Services.Account
{
    /// <summary>
    ///     getnewaddress
    ///     create a new address
    /// 
    ///     Parameters
    ///     None
    /// 
    ///     Returns
    ///     newly generated address
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getnewaddress","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": "AVHcdW3FGKbPWGHNhkPjgVgi4GGndiCxdo"
    /// }
    /// </summary>
    public class NeoGetNewAddress : RpcRequestResponseHandler<string>
    {
        public NeoGetNewAddress(IClient client) : base(client, ApiMethods.getnewaddress.ToString())
        {
        }
        
        public Task<string> SendRequestAsync(object id = null)
        {
            return base.SendRequestAsync(id);
        }
    }
}