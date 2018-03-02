using Neo.JsonRpc.Client;
using Neo.RPC.Services.Node;

namespace Neo.RPC.Services
{
    public class NeoApiNodeService : RpcClientWrapper
    {
        public NeoApiNodeService(IClient client) : base(client)
        {
            GetConnectionCount = new NeoGetConnectionCount(client);
            GetRawMemPool = new NeoGetRawMemPool(client);
        }

        public NeoGetConnectionCount GetConnectionCount { get; private set; }
        public NeoGetRawMemPool GetRawMemPool { get; private set; }
    }
}
