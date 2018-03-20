using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;
using Xunit;

namespace NeoModules.RPC.Tests.Testers.Transactions
{
	public class NeoSendManyTester : RpcRequestTester<DTOs.Transaction> // TODO: add a way to test method that need an open wallet
	{
		[Fact]
		public async void ShouldReturnTransactionDetails()
		{
			var result = await ExecuteAsync();
			Assert.NotNull(result);
		}

		public override async Task<DTOs.Transaction> ExecuteAsync(IClient client)
		{
			var sendMany = new NeoSendMany(client);
			var parameters = new List<DTOs.SendManyParameter>
			{
				new DTOs.SendManyParameter
				{
					Asset = "025d82f7b00a9ff1cfe709abe3c4741a105d067178e645bc3ebad9bc79af47d4",
					Address = "AbRTHXb9zqdqn5sVh4EYpQHGZ536FgwCx2",
					Value = 1
				},
				new DTOs.SendManyParameter
				{
					Asset = "025d82f7b00a9ff1cfe709abe3c4741a105d067178e645bc3ebad9bc79af47d4",
					Address = "AbRTHXb9zqdqn5sVh4EYpQHGZ536FgwCx2",
					Value = 1
				}
			};

			return await sendMany.SendRequestAsync(parameters);
		}

		public override Type GetRequestType()
		{
			return typeof(DTOs.Transaction);
		}
	}
}
