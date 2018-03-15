using NeoModules.JsonRpc.Client;
using System;
using System.Threading.Tasks;

namespace NeoModules.RPC.Services.Transactions
{
 //   public class NeoSendMany : RpcRequestResponseHandler<DTOs.Transaction>
	//{

	//	public NeoSendMany(IClient client) : base(client, ApiMethods.sendmany.ToString())
	//	{
	//	}

	//	public Task<DTOs.Transaction> SendRequestAsync(string assetId, string address, double amount, object id = null)
	//	{
	//		if (string.IsNullOrEmpty(assetId)) throw new ArgumentNullException(nameof(assetId));
	//		if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
	//		if (amount <= 0) throw new ArgumentException("Amount must be greater than 0", nameof(amount));

	//		return base.SendRequestAsync(id, assetId, address, amount);
	//	}

	//	public RpcRequest BuildRequest(string assetId, string address, double amount, object id = null)
	//	{
	//		if (string.IsNullOrEmpty(assetId)) throw new ArgumentNullException(nameof(assetId));
	//		if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
	//		if (amount <= 0) throw new ArgumentException("Amount must be greater than 0", nameof(amount));

	//		return base.BuildRequest(id, assetId, address, amount);
	//	}
	//}
}
