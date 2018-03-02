using Neo.JsonRpc.Client;
using Neo.RPC.Services.Block;

namespace Neo.RPC.Services
{
    public class NeoApiBlockService : RpcClientWrapper
    {
        public NeoGetBlockHash GetBlockHash { get; private set; }
        public NeoGetBestBlockHash GetBestBlockHash { get; private set; }
        public NeoGetBlock GetBlock { get; private set; }
        public NeoGetBlockCount GetBlockCount { get; private set; }
        public NeoGetBlockSerialized GetBlockSerialized { get; private set; }
        public NeoGetBlockSysFee GetBlockSysFee { get; private set; }

        public NeoApiBlockService(IClient client) : base(client)
        {           
            GetBestBlockHash = new NeoGetBestBlockHash(client);
            GetBlock = new NeoGetBlock(client);
            GetBlockSerialized = new NeoGetBlockSerialized(client);
            GetBlockCount = new NeoGetBlockCount(client);
            GetBlockHash = new NeoGetBlockHash(client);
            GetBlockSysFee = new NeoGetBlockSysFee(client);
        }
    }
}
