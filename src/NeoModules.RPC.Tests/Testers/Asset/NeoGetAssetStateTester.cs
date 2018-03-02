using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Asset;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
{
    public class NeoGetAssetStateTester : RpcRequestTester<AssetState>
    {
        [Fact]
        public async void ShouldReturnAssetState()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<AssetState> ExecuteAsync(IClient client)
        {
            var assetState = new NeoGetAssetState(client);
            return await assetState.SendRequestAsync(Settings.GetGoverningAssetHash());
        }

        public override Type GetRequestType()
        {
            return typeof(AssetState);
        }
    }
}
