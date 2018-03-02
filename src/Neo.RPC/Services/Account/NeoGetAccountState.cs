using Neo.JsonRpc.Client;
using System;
using System.Threading.Tasks;
using Neo.RPC.DTOs;

namespace Neo.RPC.Services.Account
{
    /// <summary>
    ///     getaccountstate 
    ///     Queries the account asset information, according to the account address
    /// 
    ///     Parameters
    ///     Account Address: A 34-bit length string beginning with A, such as AJBENSwajTzQtwyJFkiJSv7MAaaMc7DsRz.
    /// 
    ///     Returns
    ///     Script_hash: Contract scipt hash; All accounts in NEO are contract accounts
    ///     Frozen: Determine if the account is frozen
    ///     Votes: Query the amount of NEO on that address used to vote
    ///     Balance: Balance of assets at the address
    ///     Asset: Asset ID
    ///     Value: Amount of Assets
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getaccountstate","params":["AJBENSwajTzQtwyJFkiJSv7MAaaMc7DsRz"],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": {
    ///         "version": 0,
    ///         "script_hash": "1179716da2e9523d153a35fb3ad10c561b1e5b1a",
    ///         "frozen": false,
    ///         "votes": [],
    ///         "balances": [
    ///             {
    ///                 "asset": "0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b",
    ///                 "value": "94"
    ///             }
    ///         ]
    ///     }
    /// }
    /// </summary>
    public class NeoGetAccountState : RpcRequestResponseHandler<AccountState>
    {
        public NeoGetAccountState(IClient client) : base(client, ApiMethods.getaccountstate.ToString())
        {
        }

        public Task<AccountState> SendRequestAsync(string address, object id = null)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));
            return base.SendRequestAsync(id, address);
        }

        public RpcRequest BuildRequest(string signedTransactionData, object id = null)
        {
            if (signedTransactionData == null) throw new ArgumentNullException(nameof(signedTransactionData));
            return base.BuildRequest(id, signedTransactionData);
        }
    }
}