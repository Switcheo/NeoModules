using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.Services.Block;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoGetBlockHashTester : RpcRequestTester<string>
    {
        [Fact]
        public async Task ShouldReturnBlockHash()
        {
            var result = await ExecuteAsync();
            Assert.True(!string.IsNullOrEmpty(result));
            Assert.StartsWith("0x", result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var blockHash = new NeoGetBlockHash(client);
            return await blockHash.SendRequestAsync(10000);
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
