using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Account;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Account
{
    public class NeoGetNewAddressTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var newAddress = new NeoGetNewAddress(client);
            return await newAddress.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
