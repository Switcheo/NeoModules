using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Services.Nep5
{
    public class TokenSymbol : RpcRequestResponseHandler<DTOs.Invoke>
    {
        private readonly string _tokenScriptHash;

        public TokenSymbol(IClient client, string tokenScriptHash) : base(client, ApiMethods.invokefunction.ToString())
        {
            _tokenScriptHash = tokenScriptHash;
        }

        public Task<DTOs.Invoke> SendRequestAsync(object id = null)
        {
            return base.SendRequestAsync(id, _tokenScriptHash, Nep5Methods.symbol.ToString());
        }

        public RpcRequest BuildRequest(object id = null)
        {
            return base.BuildRequest(id, _tokenScriptHash, Nep5Methods.symbol.ToString());
        }
    }
}
