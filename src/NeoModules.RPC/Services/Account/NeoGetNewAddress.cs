using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Infrastructure;

namespace NeoModules.RPC.Services.Account
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
    public class NeoGetNewAddress : GenericRpcRequestResponseHandlerNoParam<string>
	{
        public NeoGetNewAddress(IClient client) : base(client, ApiMethods.getnewaddress.ToString())
        {
        }
    }
}