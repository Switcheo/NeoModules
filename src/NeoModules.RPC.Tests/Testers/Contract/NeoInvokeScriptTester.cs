using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Contract;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
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
