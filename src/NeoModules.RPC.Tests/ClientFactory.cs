using System;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Tests
{
    public class ClientFactory
    {
        public static IClient GetClient(TestSettings settings)
        {
            var url = settings.GetRpcUrl();
            return new RpcClient(new Uri(url));
        }
    }
}
