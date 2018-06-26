using NeoModules.RPC.Infrastructure;
using System.Collections.Generic;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Services.Node
{
    /// <Summary>
    ///     getvalidators    
    ///     Get current NEO consensus node information and voting conditions.
    /// 
    ///     Parameters
    ///     none
    /// 
    ///     Returns
    ///     Information about node voting 
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getvalidators","params":[],"id":1}'
    /// 
    ///    {
    ///    "jsonrpc": "2.0",
    ///    "id": 1,
    ///    "result": [
    ///      {
    ///          "publickey": "02486fd15702c4490a26703112a5cc1d0923fd697a33406bd5a1c00e0013b09a70",
    ///          "votes": "46632420",
    ///          "active": true
    ///      },
    ///      {
    ///          "publickey": "024c7b7fb6c310fccf1ba33b082519d82964ea93868d676662d4a59ad548df0e7d",
    ///          "votes": "46632420",
    ///          "active": true
    ///      },
    ///      {
    ///          "publickey": "025bdf3f181f53e9696227843950deb72dcd374ded17c057159513c3d0abe20b64",
    ///          "votes": "0",
    ///          "active": false
    ///      },
    ///      {
    ///          "publickey": "02aaec38470f6aad0042c6e877cfd8087d2676b0f516fddd362801b9bd3936399e",
    ///          "votes": "46632420",
    ///          "active": true
    ///      }
    ///     ]
    ///     }
    /// </Summary>
    public class NeoGetValidators : GenericRpcRequestResponseHandlerNoParam<List<DTOs.Validator>>
    {
        public NeoGetValidators(IClient client) : base(client, ApiMethods.getvalidators.ToString())
        {
        }
    }
}
