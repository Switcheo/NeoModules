using NeoModules.JsonRpc.Client;
using NeoModules.RPC;
using System;

namespace NeoModulesXF.ViewModels
{
	public class BaseViewModel
	{
		protected NeoApiService NeoService { get; set; }

		public BaseViewModel()
		{
			var rpc = new RpcClient(new Uri("http://seed1.redpulse.com:10332"));
			NeoService = new NeoApiService(rpc);
		}
	}
}
