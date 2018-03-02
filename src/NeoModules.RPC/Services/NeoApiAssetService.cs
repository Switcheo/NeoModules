using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Asset;

namespace NeoModules.RPC.Services
{
    public class NeoApiAssetService : RpcClientWrapper
    {
        public NeoApiAssetService(IClient client) : base(client)
        {
            GetAssetState = new NeoGetAssetState(client);
        }

        public NeoGetAssetState GetAssetState { get; private set; }
    }
}
