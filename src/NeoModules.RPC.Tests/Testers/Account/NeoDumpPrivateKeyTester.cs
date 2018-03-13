using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Account;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Account
{
    public class NeoDumpPrivateKeyTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnPrivateKey()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var dumpPrivateKey = new NeoDumpPrivateKey(client);
            return await dumpPrivateKey.SendRequestAsync(Settings.GetDefaultAccount());
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
