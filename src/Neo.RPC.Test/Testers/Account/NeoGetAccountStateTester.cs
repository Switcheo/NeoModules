using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Account;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoGetAccountStateTester : RpcRequestTester<AccountState>
    {
        [Fact]
        public async void ShouldReturnAccountState()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<AccountState> ExecuteAsync(IClient client)
        {
            var accountState = new NeoGetAccountState(client);
            return await accountState.SendRequestAsync(Settings.GetDefaultAccount());
        }

        public override Type GetRequestType()
        {
            return typeof(AssetState);
        }
    }
}