using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Node;

namespace NeoModules.RPC.Services
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
