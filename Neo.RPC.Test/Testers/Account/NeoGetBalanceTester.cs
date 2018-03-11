using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Account;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Neo.RPC.Tests.Testers.Account
{
    public class NeoGetBalanceTester : RpcRequestTester<WalletBalance>
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<WalletBalance> ExecuteAsync(IClient client)
        {
            var balance = new NeoGetBalance(client);
            return await balance.SendRequestAsync(Settings.GetGoverningAssetHash());
        }

        public override Type GetRequestType()
        {
            return typeof(WalletBalance);
        }
    }
}
