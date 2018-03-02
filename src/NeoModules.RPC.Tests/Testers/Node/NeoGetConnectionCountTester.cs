using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Node;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
{
    public class NeoGetConnectionCountTester : RpcRequestTester<int>
	{
		[Fact]
		public async void ShouldReturnConnectionCount()
		{
			var result = await ExecuteAsync();
			Assert.True(result >= 0);
		}

		public override async Task<int> ExecuteAsync(IClient client)
		{
			var connectionCount = new NeoGetConnectionCount(client);
			return await connectionCount.SendRequestAsync();
		}

		public override Type GetRequestType()
		{
			return typeof(int);
		}
	}
}
