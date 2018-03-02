using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Contract;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoInvokeContractTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnInvokeContractResult()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var invoke = new NeoInvokeContract(client);
            var parametersList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "String",
                    Value = "name"
                },
                new InvokeParameter
                {
                    Type = "Boolean",
                    Value = "false"
                }
            };
            return await invoke.SendRequestAsync("dc675afc61a7c0f7b3d2682bf6e1d8ed865a0e5f", parametersList);
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
