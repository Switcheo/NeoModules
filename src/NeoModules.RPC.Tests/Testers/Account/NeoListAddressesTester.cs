using System;
using System.Threading.Tasks;
using Xunit;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Account;

namespace NeoModules.RPC.Tests.Testers.Account
{
    public class NeoListAddressesTester : RpcRequestTester<WalletAddress[]>
    {
        [Fact]
        public async void ShouldReturnWalletBalance()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<WalletAddress[]> ExecuteAsync(IClient client)
        {
            var listAddresses = new NeoListAddresses(client);
            return await listAddresses.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(WalletAddress[]);
        }
    }
}

