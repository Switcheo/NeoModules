using System;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using System.Threading.Tasks;
using Xunit;
using Neo.RPC.Services;

namespace Neo.RPC.Tests.Testers.Account
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

