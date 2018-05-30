using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Services.Nep5
{
    public class TokenBalanceOf : RpcRequestResponseHandler<DTOs.Invoke>
    {
        private string _tokenScriptHash;

        public TokenBalanceOf(IClient client, string tokenScriptHash) : base(client, ApiMethods.invokefunction.ToString())
        {
            _tokenScriptHash = tokenScriptHash;
        }

        public void ChangeScriptHash(string tokenScripHash)
        {
            _tokenScriptHash = tokenScripHash;
        }

        public Task<DTOs.Invoke> SendRequestAsync(string account, object id = null)
        {
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException(nameof(account));
            var param = new List<DTOs.Stack>
            {
                new DTOs.Stack
                {
                    Type = "Hash160",
                    Value = account
                }
            };
            return base.SendRequestAsync(id, _tokenScriptHash, Nep5Methods.balanceOf.ToString(), param);
        }

        public RpcRequest BuildRequest(string account, object id = null)
        {
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException(nameof(account));
            var param = new List<DTOs.Stack>
            {
                new DTOs.Stack
                {
                    Type = "Hash160",
                    Value = account
                }
            };
            return base.BuildRequest(id, _tokenScriptHash, Nep5Methods.balanceOf.ToString(), param);
        }
    }
}
