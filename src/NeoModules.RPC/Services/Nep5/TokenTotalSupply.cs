using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Services.Nep5
{
    public class TokenTotalSupply : RpcRequestResponseHandler<DTOs.Invoke>
    {
        public TokenTotalSupply(IClient client) : base(client, ApiMethods.invokefunction.ToString())
        {
        }

        public Task<DTOs.Invoke> SendRequestAsync(string tokenScriptHash, object id = null)
        {
            return base.SendRequestAsync(id, tokenScriptHash, Nep5Methods.totalSupply.ToString());
        }

        public RpcRequest BuildRequest(string tokenScriptHash, object id = null)
        {
            return base.BuildRequest(id, tokenScriptHash, Nep5Methods.totalSupply.ToString());
        }
    }
}
