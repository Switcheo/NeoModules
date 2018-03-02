using Neo.JsonRpc.Client;
using Neo.RPC.Services.Account;

namespace Neo.RPC.Services
{
    public class NeoApiAccountService : RpcClientWrapper
    {
        public NeoApiAccountService(IClient client) : base(client)
        {
            GetAccountState = new NeoGetAccountState(client);
            ValidateAddress = new NeoValidateAddress(client);
        }
       
        public NeoGetAccountState GetAccountState { get; private set; }
        public NeoValidateAddress ValidateAddress { get; private set; }
    }
}
