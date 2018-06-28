using NeoModules.JsonRpc.Client;
using NeoModules.RPC;
using System;
using NeoModules.Rest.Services;

namespace NeoModulesXF.ViewModels
{
	public class BaseViewModel
	{
		protected NeoApiService NeoService { get; set; }
        protected NeoScanRestService NeoScanService { get; set; }

		public BaseViewModel()
		{
			var rpc = new RpcClient(new Uri("http://seed1.redpulse.com:10332"));
            NeoScanService= new NeoScanRestService(NeoScanNet.MainNet);
			NeoService = new NeoApiService(rpc);
		}
	}
}
