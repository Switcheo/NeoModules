using NeoModules.JsonRpc.Client;
using System;
using System.Threading.Tasks;

namespace NeoModules.RPC.Services.Account
{
	/// <Summary>
	///     dumpprivkey 
	///		Exports the private key of the specified address.
	///     <note type="important">
	///      You need to open the wallet in the NEO-CLI node before you execute this command.
	///     </note>
	/// 
	///     Parameters
	///     Address: To export the addresses of the private key, the address is required as a standard address.
	/// 
	///     Example
	///     Request
	///     curl -X POST --data '{ "jsonrpc": "2.0",  "method": "dumpprivkey", "params": ["ASMGHQPzZqxFB2yKmzvfv82jtKVnjhp1ES"], "id": 1}
	/// 
	///     Result
	///     {
	///			"jsonrpc": "2.0",
	///			"id": 1,
	///			"result": "L3FdgAisCmV******************************9XM65cvjYQ1"
	///		}
	/// </Summary>   
	public class NeoDumpPrivateKey : RpcRequestResponseHandler<string>
	{
		public NeoDumpPrivateKey(IClient client) : base(client, ApiMethods.dumpprivkey.ToString())
		{
		}

		public Task<string> SendRequestAsync(string address, object id = null)
		{
			if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
			return base.SendRequestAsync(id, address);
		}

		public RpcRequest BuildRequest(string address, object id = null)
		{
			if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
			return base.BuildRequest(id, address);
		}
	}
}
