using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.Services.Contract;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
	public class NeoGetStorageTester : RpcRequestTester<string> //todo
	{
		[Fact]
		public async void ShouldReturnStorage()
		{
			var result = await ExecuteAsync();
			Assert.Null(result);
		}

		public override async Task<string> ExecuteAsync(IClient client)
		{
			var contractState = new NeoGetStorage(client);
			return await contractState.SendRequestAsync("03febccf81ac85e3d795bc5cbd4e84e907812aa3", "5065746572");
		}

		public override Type GetRequestType()
		{
			return typeof(string);
		}
	}
}
