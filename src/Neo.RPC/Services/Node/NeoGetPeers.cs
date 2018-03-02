using Neo.JsonRpc.Client;
using Neo.RPC.Infrastructure;

namespace Neo.RPC.Services.Node
{
    /// <Summary>
    ///     getpeers  
    ///     Gets the list of nodes that the node is currently connected/disconnected from.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     List of nodes
    ///     Unconnected: A node that is not currently connected.
    ///     Bad: Nodes that are no longer connected.
    ///     Connected: the node to which you are currently connected.
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getpeers","params":[],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "id":1,
    ///     "jsonrpc": "2.0",
    ///     "result": {
    ///         "unconnected": [
    ///             {
    ///                 "address": "::ffff:70.73.16.236",
    ///                 "port": 10333
    ///             },
    ///             {
    ///                 "address": "::ffff:82.95.77.148",
    ///                 "port": 10333
    ///             },
    ///         ],
    ///         "bad": [
    ///             {
    ///                 "address": "::ffff:139.219.226.107",
    ///                 "port": 10333
    ///             }
    ///         ],
    ///         "connected": [
    ///             {
    ///                 "address": "::ffff:139.219.106.33",
    ///                 "port": 10333
    ///             },
    ///             {
    ///                 "address": "::ffff:47.88.53.224",
    ///                 "port": 10333
    ///             }
    ///         ]
    ///      }
    /// }
    /// </Summary>
    public class NeoGetPeers : GenericRpcRequestResponseHandlerNoParam<DTOs.Peers>
    {
        public NeoGetPeers(IClient client) : base(client, ApiMethods.getpeers.ToString())
        {
        }
    }
}