using System.Threading.Tasks;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Services.Nep5
{
    public class TokenTotalSupply : RpcRequestResponseHandler<DTOs.Invoke>
    {
		private readonly string _tokenScriptHash;

		public TokenTotalSupply(IClient client, string tokenScriptHash) : base(client, ApiMethods.invokefunction.ToString())
        {
			_tokenScriptHash = tokenScriptHash;
		}

		public Task<DTOs.Invoke> SendRequestAsync(object id = null)
		{
			return base.SendRequestAsync(id, _tokenScriptHash, Nep5Methods.totalSupply.ToString());
		}

		public RpcRequest BuildRequest(object id = null)
		{
			return base.BuildRequest(id, _tokenScriptHash, Nep5Methods.totalSupply.ToString());
		}
	}
}
