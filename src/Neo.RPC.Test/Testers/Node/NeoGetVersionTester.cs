using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Node;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
    public class NeoGetVersionTester : RpcRequestTester<Version>
	{
		[Fact]
		public async void ShouldReturnVersion()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<Version> ExecuteAsync(IClient client)
		{
			var version = new NeoGetVersion(client);
			return await version.SendRequestAsync();
		}

		public override System.Type GetRequestType()
		{
			return typeof(Version);
		}
	}
}
