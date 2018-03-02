using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Services.Contract
{
	/// <Summary>
	///     invoke    
	///     Returns the result after calling a smart contract at scripthash with the given parameters.
	///     <note type="important">
	///     This method is to test your VM script as if they were ran on the blockchain at that point in time. This RPC call does not affect the blockchain in any way.
	///     </note>
	/// 
	///     Parameters
	///     Scripthash: Smart contract scripthash 
	///     Params: The parameters to be passed into the smart contract
	/// 
	///     Returns
	///     Information about smart contract invocation
	/// 
	///     Example
	///     Request
	///     curl -X POST --data '{"jsonrpc":"2.0","method":"invoke",
	///         "params":["dc675afc61a7c0f7b3d2682bf6e1d8ed865a0e5f",
	///             [
	///                 {
	///                     "type": "String",
	///                     "value": "name"
	///                 },
	///                 {
	///                     "type":"Boolean",
	///                     "value": false
	///                 }
	///             ]
	///         ],"id":1}'
	/// 
	///     Result
	/// {
	///     "jsonrpc": "2.0",
	///     "id": 1,
	///     "result": {
	///         "state": "HALT, BREAK",
	///         "gas_consumed": "2.489",
	///         "stack": [
	///             {
	///                 "type": "ByteArray",
	///                 "value": "576f6f6c6f6e67"
	///             }
	///         ]
	///     }
	/// }
	/// </Summary>   
	public class NeoInvokeContract : RpcRequestResponseHandler<DTOs.Invoke>
	{
		public NeoInvokeContract(IClient client) : base(client, ApiMethods.invoke.ToString())
		{
		}
		public Task<DTOs.Invoke> SendRequestAsync(string scriptHash, List<DTOs.InvokeParameter> parameters, object id = null)
		{
			if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
			return base.SendRequestAsync(id, scriptHash, parameters);
		}

		public RpcRequest BuildRequest(string scriptHash, List<DTOs.InvokeParameter> parameters, object id = null)
		{
			if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));

			return base.BuildRequest(id, scriptHash, parameters);
		}
	}
}