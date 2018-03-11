using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Transactions
{
   public  class NeoGetRawTransactionSerializedTester : RpcRequestTester<string>
    {
        [Fact]
        public async void ShouldReturnRawTransaction()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var rawTransaction = new NeoGetRawTransactionSerialized(client);
            return await rawTransaction.SendRequestAsync("f4250dab094c38d8265acc15c366dc508d2e14bf5699e12d9df26577ed74d657");
        }

        public override Type GetRequestType()
        {
            return typeof(string);
        }
    }
}
