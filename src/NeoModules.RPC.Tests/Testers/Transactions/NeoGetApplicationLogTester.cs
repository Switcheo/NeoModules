using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Transactions;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Transactions
{
    // todo add a node that has applicationlog enabled
    public class NeoGetApplicationLogTester : RpcRequestTester<ApplicationLog>
    {
        [Fact]
        public async void ShouldReturnApplicationLog()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<ApplicationLog> ExecuteAsync(IClient client)
        {
            var applicationLog = new NeoGetApplicationLog(client);
            return await applicationLog.SendRequestAsync(Settings.GetContractTransaction());
        }

        public override Type GetRequestType()
        {
            return typeof(ApplicationLog);
        }
    }
}
