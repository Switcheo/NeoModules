using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Account;

namespace NeoModules.RPC.Services
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
