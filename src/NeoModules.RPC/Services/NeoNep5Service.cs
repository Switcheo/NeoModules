using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Nep5;

namespace NeoModules.RPC.Services
{
    public class NeoNep5Service : RpcClientWrapper
    {
        public string TokenScriptHash { get; set; }

        public NeoNep5Service(IClient client, string tokenScriptHash) : base(client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (string.IsNullOrEmpty(tokenScriptHash)) throw new ArgumentNullException(nameof(tokenScriptHash));

            TokenScriptHash = tokenScriptHash;
            GetTokenBalance = new TokenBalanceOf(client, tokenScriptHash);
            GetTokenDecimals = new TokenDecimals(client, tokenScriptHash);
            GetTokenName = new TokenName(client, tokenScriptHash);
            GetTokenTotalSupply = new TokenTotalSupply(client, tokenScriptHash);
            GetTokenSymbol = new TokenSymbol(client, tokenScriptHash);
        }

        //TODO: can refractor this more
        public void ChangeTokenScripHash(string tokenScriptHash)
        {
            if (string.IsNullOrEmpty(tokenScriptHash)) throw new ArgumentNullException(nameof(tokenScriptHash));

            TokenScriptHash = tokenScriptHash;
            GetTokenBalance.ChangeScriptHash(tokenScriptHash);
            GetTokenDecimals.ChangeScriptHash(tokenScriptHash);
            GetTokenName.ChangeScriptHash(tokenScriptHash);
            GetTokenTotalSupply.ChangeScriptHash(tokenScriptHash);
            GetTokenSymbol.ChangeScriptHash(tokenScriptHash);
        }

        public async Task<string> GetName()
        {
            var result = await GetTokenName.SendRequestAsync().ConfigureAwait(false);
            return result.Stack[0].Value.ToString();
        }

        public async Task<string> GetSymbol()
        {
            var result = await GetTokenSymbol.SendRequestAsync().ConfigureAwait(false);
            return result.Stack[0].Value.ToString();
        }

        public async Task<string> GetTotalSupply()
        {
            var result = await GetTokenTotalSupply.SendRequestAsync().ConfigureAwait(false);
            return result.Stack[0].Value.ToString();
        }

        public async Task<string> GetDecimals()
        {
            var result = await GetTokenDecimals.SendRequestAsync().ConfigureAwait(false);
            return result.Stack[0].Value.ToString();
        }

        public async Task<string> GetBalance(string addressScriptHash)
        {
            var result = await GetTokenBalance.SendRequestAsync(addressScriptHash).ConfigureAwait(false);
            return result.Stack[0].Value.ToString();
        }

        private TokenBalanceOf GetTokenBalance { get; }
        private TokenDecimals GetTokenDecimals { get; }
        private TokenName GetTokenName { get; }
        private TokenTotalSupply GetTokenTotalSupply { get; }
        private TokenSymbol GetTokenSymbol { get; }
    }
}
