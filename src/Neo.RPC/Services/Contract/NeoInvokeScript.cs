using Neo.JsonRpc.Client;
using System;
using System.Threading.Tasks;

namespace Neo.RPC.Services.Contract
{
	/// <Summary>
	///     invokescript   
	///     Returns the result after passing a script through the VM.
	/// 
	///     Parameters
	///     Script: A script runnable by the VM. This is the same script that is carried in InvocationTransaction
	/// 
	///     Returns
	///     Result after passing a script through the VM.
	/// 
	///     Example
	///     Request
	///     curl -X POST --data '{"jsonrpc":"2.0","method":"invokescript","params":[00046e616d656711c4d1f4fba619f2628870d36e3a9773e874705b],"id":1}'
	/// 
	///     Result
	///     {
	///     "jsonrpc": "2.0",
	///     "id": 1,
	///     "result": {
	///			"state": "HALT, BREAK",
	///			"gas_consumed": "0.125",
	///			"stack": [
	///				{
	///					"type": "ByteArray",
	///					"value": "5265642050756c736520546f6b656e20332e312e34"
	///				}
	///				]
	///			}
	///     }
	/// </Summary>
	public class NeoInvokeScript : RpcRequestResponseHandler<DTOs.Invoke>
	{
		public NeoInvokeScript(IClient client) : base(client, ApiMethods.invokescript.ToString())
		{
		}

		public Task<DTOs.Invoke> SendRequestAsync(string scriptHash, object id = null)
		{
			if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
			return base.SendRequestAsync(id, scriptHash);
		}

		public RpcRequest BuildRequest(string scriptHash, object id = null)
		{
			if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
			return base.BuildRequest(id, scriptHash);
		}
	}
}
