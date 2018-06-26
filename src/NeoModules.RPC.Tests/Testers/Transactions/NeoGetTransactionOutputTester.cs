using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Transactions;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Transactions
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
                "7f7f3b361e46b271e15c640d40994f759ce13f608ac53fd970b9d6db779dd589");
        }

        public override Type GetRequestType()
        {
            return typeof(TransactionOutput);
        }
    }
}