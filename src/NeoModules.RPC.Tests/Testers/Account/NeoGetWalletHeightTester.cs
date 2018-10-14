using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Account;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Account
{
    public class NeoGetWalletHeightTester : RpcRequestTester<long>
    {
        [Fact]
        public async Task ShouldReturnWalletHeight()
        {
            var result = await ExecuteAsync();
            Assert.True(result > 0);
        }

        public override async Task<long> ExecuteAsync(IClient client)
        {
            var walletHeight = new NeoGetWalletHeight(client);
            return await walletHeight.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(long);
        }
    }
}
