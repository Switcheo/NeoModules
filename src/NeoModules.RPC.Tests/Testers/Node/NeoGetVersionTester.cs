using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Node;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Node
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
