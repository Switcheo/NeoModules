using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;

namespace Neo.RPC.Services.Account
{
    /// <summary>
    ///     validateaddress 
    ///     Verifies that the address is a correct NEO address.
    /// 
    ///     Parameters
    ///     address: Address.
    /// 
    ///     Returns
    ///     Address: Contract scipt hash; All accounts in NEO are contract accounts
    ///     IsValid: bool indicating if address is valid or not
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"validateaddress","params":["AQVh2pG732YvtNaxEGkQUei3YA4cvo7d2i"],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": {
    ///         "address": "AQVh2pG732YvtNaxEGkQUei3YA4cvo7d2i",
    ///         "isvalid": true
    ///     }
    /// }
    /// </summary>
    public class NeoValidateAddress : RpcRequestResponseHandler<ValidateAddress>
    {
        public NeoValidateAddress(IClient client) : base(client, ApiMethods.validateaddress.ToString())
        {
        }

        public Task<ValidateAddress> SendRequestAsync(string address, object id = null)
        {
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
            return base.SendRequestAsync(id, address);
        }

        public RpcRequest BuildRequest(string address, object id = null)
        {
            if (string.IsNullOrEmpty(address)) throw new ArgumentOutOfRangeException(nameof(address));
            return base.BuildRequest(id, address);
        }
    }
}