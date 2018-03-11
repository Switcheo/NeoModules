using Neo.JsonRpc.Client;
using Neo.RPC.Services;
using System;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace Neo.RPC.Tests.Testers.Transactions
{
    public class NeoSendAssetsTester : RpcRequestTester<Transaction>
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
