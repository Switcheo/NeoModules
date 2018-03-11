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
            GetNewAddress = new NeoGetNewAddress(client);
            GetBalance = new NeoGetBalance(client);
            ListAddresses = new NeoListAddresses(client);
        }
       
        public NeoGetAccountState GetAccountState { get; private set; }
        public NeoValidateAddress ValidateAddress { get; private set; }
        public NeoGetNewAddress GetNewAddress { get; private set; }
        public NeoGetBalance GetBalance { get; private set; }
        public NeoListAddresses ListAddresses { get; private set; }
    }
}
