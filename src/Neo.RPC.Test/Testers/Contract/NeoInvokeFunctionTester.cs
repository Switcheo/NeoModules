using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Contract;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoInvokeFunctionTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnInvokeFunctionResult()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var invokeFunction = new NeoInvokeFunction(client);
            var parametersList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "Hash160",
                    Value = "bfc469dd56932409677278f6b7422f3e1f34481d"
                }
            };
            return await invokeFunction.SendRequestAsync("ecc6b20d3ccac1ee9ef109af5a7cdb85706b1df9", "balanceOf", parametersList);
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
