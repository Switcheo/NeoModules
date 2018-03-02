using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Services.Contract
{
    /// <Summary>
    ///     getstorage   
    ///     Returns the stored value, according to the contract script hash and the stored key.
    /// 
    ///     Parameters
    ///     Script hash: Contract script hash
    ///     Key: The key to look up in storage (in hex string)
    /// 
    ///     Returns
    ///     string: Stored value
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getstorage","params":["03febccf81ac85e3d795bc5cbd4e84e907812aa3", "5065746572"],"id":1}'
    /// 
    ///     Result
    ///     {
    ///         "jsonrpc": "2.0",
    ///         "id": 15,
    ///         "result": "4c696e"
    ///     }
    /// </Summary>
    public class NeoGetStorage : RpcRequestResponseHandler<string>
    {
        public NeoGetStorage(IClient client) : base(client, ApiMethods.getstorage.ToString())
        {
        }

        public Task<string> SendRequestAsync(string scriptHash, string hexKey, object id = null)
        {
            if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
            if (string.IsNullOrEmpty(hexKey)) throw new ArgumentNullException(nameof(hexKey));
            return base.SendRequestAsync(id, scriptHash, hexKey);
        }

        public RpcRequest BuildRequest(string scriptHash, string hexKey, object id = null)
        {
            if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentOutOfRangeException(nameof(scriptHash));
            if (string.IsNullOrEmpty(hexKey)) throw new ArgumentNullException(nameof(hexKey));
            return base.BuildRequest(id, scriptHash, hexKey);
        }
    }
}