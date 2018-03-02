using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Transactions;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoGetTransactionOutputTester : RpcRequestTester<TransactionOutput>
    {
        [Fact]
        public async void ShouldReturnTransactionOutput()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<TransactionOutput> ExecuteAsync(IClient client)
        {
            var transactionOutput = new NeoGetTransactionOutput(client);
            return await transactionOutput.SendRequestAsync(
                "f4250dab094c38d8265acc15c366dc508d2e14bf5699e12d9df26577ed74d657"); // todo move to settings
        }

        public override Type GetRequestType()
        {
            return typeof(TransactionOutput);
        }
    }
}