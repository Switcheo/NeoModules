using Neo.JsonRpc.Client;
using Neo.RPC.Services;

namespace Neo.RPC
{
    public class NeoApiService : RpcClientWrapper
    {
        public NeoApiService(IClient client) : base(client)
        {
            Client = client;

            Accounts = new NeoApiAccountService(client);
            Assets = new NeoApiAssetService(client);
            Blocks = new NeoApiBlockService(client);
            Contracts = new NeoApiContractService(client);
            Nodes = new NeoApiNodeService(client);
            Transactions = new NeoApiTransactionService(client);
        }

        public NeoApiAccountService Accounts { get; private set; }
        public NeoApiAssetService Assets { get; private set; }
        public NeoApiBlockService Blocks { get; private set; }
        public NeoApiContractService Contracts { get; private set; }
        public NeoApiNodeService Nodes { get; private set; }
        public NeoApiTransactionService Transactions { get; private set; }
    }
}
