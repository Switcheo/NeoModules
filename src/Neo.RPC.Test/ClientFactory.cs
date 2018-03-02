using System;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Tests
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
