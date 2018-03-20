using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Account;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Account
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
