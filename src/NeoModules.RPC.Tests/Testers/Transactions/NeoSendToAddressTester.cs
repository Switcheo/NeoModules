using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
{
    public class NeoSendToAddressTester : RpcRequestTester<DTOs.Transaction> // todo: add a way to test method that need an open wallet
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<DTOs.Transaction> ExecuteAsync(IClient client)
        {
            var sendAssets = new NeoSendToAddress(client);
            return await sendAssets.SendRequestAsync(Settings.GetGoverningAssetHash(), Settings.GetAddress(), 1);
        }

        public override Type GetRequestType()
        {
            return typeof(DTOs.Transaction);
        }
    }
}
