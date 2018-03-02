using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.Services.Block;
using Xunit;

namespace Neo.RPC.Tests.Testers
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
