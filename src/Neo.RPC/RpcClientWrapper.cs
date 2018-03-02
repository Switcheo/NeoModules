using System;
using Neo.JsonRpc.Client;

namespace Neo.RPC
{
    public class RpcClientWrapper
    {
        public RpcClientWrapper(IClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected IClient Client { get; set; }
    }
}
