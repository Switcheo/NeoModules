using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Nep5;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Nep5
{
    public class Nep5GetTotalSupplyTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnTotalSupply()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result.Stack[0].Value);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var totalSupply = new TokenTotalSupply(client, Settings.GetNep5TokenHash());
            return await totalSupply.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
