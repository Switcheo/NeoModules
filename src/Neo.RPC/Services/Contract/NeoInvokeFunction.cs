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
	///     Operation: The operation name (string)
	///     Params: The parameters to be passed into the smart contract operatio
	/// 
	///     Example
	///     Request
	///     curl -X POST --data '{"jsonrpc":"2.0","method":"invoke",
	///         "params":["ecc6b20d3ccac1ee9ef109af5a7cdb85706b1df9",
	///                 balanceOf,
	///             [
	///                 {
	///                     "type": "Hash160",
	///                     "value": "bfc469dd56932409677278f6b7422f3e1f34481d"
	///                 }
	///             ]
	///         ],
	///         "id":3}'
	/// 
	///     Result
	///     {
	///     "jsonrpc": "2.0",
	///     "id": 3,
	///     "result": {
	///         "state": "HALT, BREAK",
	///         "gas_consumed": "0.338",
	///         "stack": [
	///             {
	///                 "type": "ByteArray",
	///                 "value": "00e1f505"
	///             }
	///         ]
	///     }
	/// }
	/// </Summary>   
	public class NeoInvokeFunction : RpcRequestResponseHandler<DTOs.Invoke>
	{
		public NeoInvokeFunction(IClient client) : base(client, ApiMethods.invokefunction.ToString())
		{
		}

		public Task<DTOs.Invoke> SendRequestAsync(string scriptHash, string operation, List<DTOs.InvokeParameter> parameters, object id = null)
		{
			if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
			if (string.IsNullOrEmpty(operation)) throw new ArgumentNullException(nameof(operation));

			return base.SendRequestAsync(id, scriptHash, operation, parameters);
		}

		public RpcRequest BuildRequest(string scriptHash, string operation, List<DTOs.InvokeParameter> parameters, object id = null)
		{
			if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
			if (string.IsNullOrEmpty(operation)) throw new ArgumentNullException(nameof(operation));

			return base.BuildRequest(id, scriptHash, operation, parameters);
		}
	}
}
