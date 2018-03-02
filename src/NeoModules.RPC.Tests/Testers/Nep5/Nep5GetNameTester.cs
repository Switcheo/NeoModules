using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Nep5;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
{
    public class Nep5GetNameTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnName()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result.Stack[0].Value);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var name = new TokenName(client, Settings.GetNep5TokenHash());
            return await name.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
