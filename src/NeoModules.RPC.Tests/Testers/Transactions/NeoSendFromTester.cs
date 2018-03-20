using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Transactions
{
	public class NeoSendFromTester : RpcRequestTester<DTOs.Transaction>
	{
		[Fact]
		public async void ShouldReturnTransactionDetails()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<DTOs.Transaction> ExecuteAsync(IClient client)
		{
			var sendFrom = new NeoSendFrom(client);
			return await sendFrom.SendRequestAsync(Settings.GetGoverningAssetHash(), "FROM_ADDRESS_HERE", "TO_ADDRESS_HERE", 1);
		}

		public override Type GetRequestType()
		{
			return typeof(DTOs.Transaction);
		}
	}
}
