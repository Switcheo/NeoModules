using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;

namespace NeoModules.RPC.Services.Nep5
{
    public class TokenBalanceOf : RpcRequestResponseHandler<Invoke>
    {
        public TokenBalanceOf(IClient client) : base(client, ApiMethods.invokefunction.ToString())
        {
        }

        public Task<Invoke> SendRequestAsync(string address, string tokenScriptHash, object id = null)
        {
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
            if (string.IsNullOrEmpty(tokenScriptHash)) throw new ArgumentNullException(nameof(tokenScriptHash));
            var param = new List<Stack>
            {
                new Stack
                {
                    Type = "Hash160",
                    Value = address
                }
            };
            return base.SendRequestAsync(id, tokenScriptHash, Nep5Methods.balanceOf.ToString(), param);
        }

        public Task<Invoke> SendRequestAsync(byte[] address, string tokenScriptHash, object id = null)
        {
            if (address.Length != 20) throw new ArgumentNullException(nameof(address));
            if (string.IsNullOrEmpty(tokenScriptHash)) throw new ArgumentNullException(nameof(tokenScriptHash));
            var param = new List<Stack>
            {
                new Stack
                {
                    Type = "Hash160",
                    Value = address
                }
            };
            return base.SendRequestAsync(id, tokenScriptHash, Nep5Methods.balanceOf.ToString(), param);
        }

        public RpcRequest BuildRequest(string address, string tokenScriptHash, object id = null)
        {
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
            if (string.IsNullOrEmpty(tokenScriptHash)) throw new ArgumentNullException(nameof(tokenScriptHash));
            var param = new List<Stack>
            {
                new Stack
                {
                    Type = "Hash160",
                    Value = address
                }
            };
            return base.BuildRequest(id, tokenScriptHash, Nep5Methods.balanceOf.ToString(), param);
        }
    }
}