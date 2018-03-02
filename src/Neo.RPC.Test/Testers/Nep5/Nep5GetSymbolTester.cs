using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Nep5;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class Nep5GetSymbolTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnSymbol()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result.Stack[0].Value);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var symbol = new TokenSymbol(client, Settings.GetNep5TokenHash());
            return await symbol.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
