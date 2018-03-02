using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;
using Neo.RPC.Services.Contract;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Neo.RPC.Tests.Testers
{
	public class NeoGetContractStateTester : RpcRequestTester<ContractState>
	{
		[Fact]
		public async void ShouldReturnContractState()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<ContractState> ExecuteAsync(IClient client)
		{
			var contractState = new NeoGetContractState(client);
			return await contractState.SendRequestAsync(Settings.GetContractHash());
		}

		public override Type GetRequestType()
		{
			return typeof(ContractState);
		}
	}
}
