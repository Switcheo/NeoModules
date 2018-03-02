using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.Services.Node;
using Xunit;

namespace Neo.RPC.Tests.Testers
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
