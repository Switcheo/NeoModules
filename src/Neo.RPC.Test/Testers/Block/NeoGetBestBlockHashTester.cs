using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.Services.Block;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoGetBestBlockHashTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnBestBlockHash()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
            Assert.StartsWith("0x",result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var bestBlockHash = new NeoGetBestBlockHash(client);
            return await bestBlockHash.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
