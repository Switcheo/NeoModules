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
            GetValidators = new NeoGetValidators(client);
            GetVersion = new NeoGetVersion(client);
        }

        public NeoGetConnectionCount GetConnectionCount { get; private set; }
        public NeoGetRawMemPool GetRawMemPool { get; private set; }
        public NeoGetVersion GetVersion { get; private set; }
        public NeoGetValidators GetValidators { get; private set; }
    }
}
