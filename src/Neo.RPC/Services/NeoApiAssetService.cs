using Neo.JsonRpc.Client;
using Neo.RPC.Services.Asset;

namespace Neo.RPC.Services
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
