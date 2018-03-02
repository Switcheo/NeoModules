using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services.Contract;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NeoModules.RPC.Tests.Testers
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
