using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Node;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Neo.RPC.Tests.Testers
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
