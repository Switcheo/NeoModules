using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Block;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Block
{
    public class NeoGetBlockSerializedTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnBlockSerialized()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var blockSerialized = new NeoGetBlockSerialized(client);
            return await blockSerialized.SendRequestAsync(1005434);
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
