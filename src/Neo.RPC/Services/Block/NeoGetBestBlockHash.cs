using Neo.JsonRpc.Client;
using Neo.RPC.Infrastructure;

namespace Neo.RPC.Services.Block
{
    /// <Summary>
    ///     getbestblockhash 
    ///     Returns the hash of the tallest block in the main chain.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     The hash of the tallest block in the main chain.
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getbestblockhash","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///    "jsonrpc": "2.0",
    ///    "id": 1,
    ///    "result": "773dd2dae4a9c9275290f89b56e67d7363ea4826dfd4fc13cc01cf73a44b0d0e"
    ///     }
    /// </Summary>
    public class NeoGetBestBlockHash : GenericRpcRequestResponseHandlerNoParam<string>
    {
        public NeoGetBestBlockHash(IClient client) : base(client, ApiMethods.getbestblockhash.ToString())
        {
        }
    }
}