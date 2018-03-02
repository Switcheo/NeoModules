using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.Services.Block;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoGetBlockSysFeeTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnBlockSysFee()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var blockSysFee = new NeoGetBlockSysFee(client);
            return await blockSysFee.SendRequestAsync(1005434);
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
