using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services;

namespace NeoModules.RPC
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
            TokenStandard = new NeoNep5Service(client);
            Nodes = new NeoApiNodeService(client);
            Transactions = new NeoApiTransactionService(client);
        }

        public NeoApiAccountService Accounts { get; }
        public NeoApiAssetService Assets { get; }
        public NeoApiBlockService Blocks { get; }
        public NeoApiContractService Contracts { get; }
        public NeoNep5Service TokenStandard { get; }
        public NeoApiNodeService Nodes { get; }
        public NeoApiTransactionService Transactions { get; }
    }
}