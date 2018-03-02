using Neo.JsonRpc.Client;
using Neo.RPC.Infrastructure;

namespace Neo.RPC.Services.Block
{
    /// <Summary>
    ///     getblockcount 
    ///     Gets the number of blocks in the main chain.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     Integer: Number of blocks in the chain
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getblockcount","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "id":1,
    ///     "jsonrpc": "2.0",
    ///     "result": 991991
    ///     }
    /// </Summary>
    public class NeoGetBlockCount : GenericRpcRequestResponseHandlerNoParam<int>
    {
        public NeoGetBlockCount(IClient client) : base(client, ApiMethods.getblockcount.ToString())
        {
        }
    }
}