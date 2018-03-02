using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Node;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
{
	public class NeoGetPeersTester : RpcRequestTester<Peers>
	{
		[Fact]
		public async void ShouldReturnPeers()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<Peers> ExecuteAsync(IClient client)
		{
			var peers = new NeoGetPeers(client);
			return await peers.SendRequestAsync();
		}

		public override Type GetRequestType()
		{
			return typeof(Peers);
		}
	}
}
