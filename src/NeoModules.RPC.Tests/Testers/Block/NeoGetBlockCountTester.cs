using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Block;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Block
{
    public class NeoGetBlockCountTester : RpcRequestTester<int>
    {
        [Fact]
        public async Task ShouldReturnBlockCount()
        {
            var result = await ExecuteAsync();
            Assert.True(result > 0);
        }

        public override async Task<int> ExecuteAsync(IClient client)
        {
            var blockCount = new NeoGetBlockCount(client);
            return await blockCount.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(int);
        }
    }
}
