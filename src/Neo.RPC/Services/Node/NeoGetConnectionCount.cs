using Neo.JsonRpc.Client;
using Neo.RPC.Infrastructure;

namespace Neo.RPC.Services.Node
{
    /// <Summary>
    ///     getconnectioncount   
    ///     Gets the current number of connections for the node.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     Number of connection on the node
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getconnectioncount","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": 10
    /// }
    /// </Summary>
    public class NeoGetConnectionCount : GenericRpcRequestResponseHandlerNoParam<int>
    {
        public NeoGetConnectionCount(IClient client) : base(client, ApiMethods.getconnectioncount.ToString())
        {
        }
    }
}