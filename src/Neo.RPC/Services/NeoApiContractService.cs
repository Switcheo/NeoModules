using Neo.JsonRpc.Client;
using Neo.RPC.Services.Contract;

namespace Neo.RPC.Services
{
	public class NeoApiContractService : RpcClientWrapper
	{
		public NeoApiContractService(IClient client) : base(client)
		{
			GetContractState = new NeoGetContractState(client);
			GetStorage = new NeoGetStorage(client);
			InvokeContract = new NeoInvokeContract(client);
			InvokeFunction = new NeoInvokeFunction(client);
			InvokeScript = new NeoInvokeScript(client);
		}

		public NeoGetContractState GetContractState { get; set; }

		public NeoGetStorage GetStorage { get; set; }

		public NeoInvokeContract InvokeContract { get; set; }

		public NeoInvokeFunction InvokeFunction { get; set; }

		public NeoInvokeScript InvokeScript { get; set; }
	}
}
