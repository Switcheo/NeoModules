using Neo.JsonRpc.Client;
using Neo.RPC.Services.Account;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Neo.RPC.Tests.Testers.Account
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
