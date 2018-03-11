using System;
using System.Threading.Tasks;
using System.Transactions;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;
using NeoModules.RPC.Tests.Testers;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
{
    public class NeoSendAssetsTester : RpcRequestTester<Transaction> // todo: add a way to test method that need an open wallet
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<Transaction> ExecuteAsync(IClient client)
        {
            var sendAssets = new NeoSendAssets(client);
            return await sendAssets.SendRequestAsync(Settings.GetGoverningAssetHash(), Settings.GetAddress(), 1);
        }

        public override Type GetRequestType()
        {
            return typeof(Transaction);
        }
    }
}
