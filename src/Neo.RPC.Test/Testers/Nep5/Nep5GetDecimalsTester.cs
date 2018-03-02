using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Nep5;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class Nep5GetDecimalsTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnDecimals()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result.Stack[0].Value);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var decimals = new TokenDecimals(client, Settings.GetNep5TokenHash());
            return await decimals.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
