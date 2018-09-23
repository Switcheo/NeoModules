using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Block;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Block
{
    public class NeoGetBlockHeaderTester : RpcRequestTester<DTOs.BlockHeader>
    {
        [Fact]
        public async void ShouldReturnBlockHeader()
        {
            var getBlock = new NeoGetBlock(Client);
            var blockByIndex = await getBlock.SendRequestAsync(Settings.GetBlockHash());
            Assert.NotNull(blockByIndex);
        }

        public override async Task<DTOs.BlockHeader> ExecuteAsync(IClient client)
        {
            var block = new NeoGetBlockHeader(client);
            return await block.SendRequestAsync("c175e66e8619cbfede76f1939e63c0aa5e5e7eb0bbecb2127d9d074d9281214d");
        }

        public override Type GetRequestType()
        {
            return typeof(DTOs.Block);
        }
    }
}
