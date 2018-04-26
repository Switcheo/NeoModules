using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Contract;

namespace NeoModules.RPC.Services
{
	public class NeoApiContractService : RpcClientWrapper
	{
		public NeoApiContractService(IClient client) : base(client)
		{
			GetContractState = new NeoGetContractState(client);
			GetStorage = new NeoGetStorage(client);
			Invoke = new NeoInvoke(client);
			InvokeFunction = new NeoInvokeFunction(client);
			InvokeScript = new NeoInvokeScript(client);
		}

		public NeoGetContractState GetContractState { get; set; }

		public NeoGetStorage GetStorage { get; set; }

		public NeoInvoke Invoke { get; set; }

		public NeoInvokeFunction InvokeFunction { get; set; }

		public NeoInvokeScript InvokeScript { get; set; }
	}
}
