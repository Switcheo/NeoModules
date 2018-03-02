using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Contract;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoInvokeScriptTester : RpcRequestTester<Invoke>
    {
        [Fact]
        public async void ShouldReturnInvokeScriptResult()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<Invoke> ExecuteAsync(IClient client)
        {
            var invokeScript = new NeoInvokeScript(client);
            return await invokeScript.SendRequestAsync(Settings.GetContractHash());
        }

        public override Type GetRequestType()
        {
            return typeof(Invoke);
        }
    }
}
