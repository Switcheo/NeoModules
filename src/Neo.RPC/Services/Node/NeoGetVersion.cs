using Neo.JsonRpc.Client;
using Neo.RPC.Infrastructure;

namespace Neo.RPC.Services.Node
{
    /// <Summary>
    ///     getversion    
    ///     Returns the version information about the queried node.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     Information about node version
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getversion","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///         "jsonrpc": "2.0",
    ///         "id": 3,
    ///         "result": {
    ///             "port": 0,
    ///             "nonce": 156443862,
    ///             "useragent": "/NEO:2.3.5/"
    ///         }
    ///     }
    /// </Summary>
    public class NeoGetVersion : GenericRpcRequestResponseHandlerNoParam<DTOs.Version> //todo dto
    {
        public NeoGetVersion(IClient client) : base(client, ApiMethods.getversion.ToString())
        {
        }
    }
}