using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Nep5;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class Nep5BalanceOfTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnBalanceOf()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result.Stack[0].Value);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var balanceOf = new TokenBalanceOf(client, Settings.GetNep5TokenHash());
            return await balanceOf.SendRequestAsync("0x0ff9070d64d19076d08947ba4a82b72709f30baf");
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
