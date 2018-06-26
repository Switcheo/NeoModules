using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Node;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Node
{
    public class NeoGetValidatorsTester : RpcRequestTester<List<Validator>>
    {
        public override async Task<List<Validator>> ExecuteAsync(IClient client)
        {
            var validators = new NeoGetValidators(client);
            return await validators.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(List<Validator>);
        }

        [Fact]
        public async void ShouldReturnValidators()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }
    }
}