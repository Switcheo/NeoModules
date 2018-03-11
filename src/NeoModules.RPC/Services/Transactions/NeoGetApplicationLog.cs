using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Services.Transactions
{
    /// <Summary>
    ///     getapplicationlog     
    ///     Returns the contract log based on the specified txid.
    /// 
    ///     Parameters
    ///     Txid: Transaction ID
    /// 
    ///     Returns
    ///     Contract Log
    ///     <note type="important">
    ///     You need to run the command dotnet neo-cli.dll --log to enable logging before invoking this method. The complete contract logs are stored under the ApplicationLogs directory.
    ///     </note>
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{ "jsonrpc": "2.0","method": "getapplicationlog","params": ["0x0d03ad35eb8b0bb2e43e18896d22cd2a77fe54fc0b00794fb295bcf96257d0e3"],"id": 1}'
    ///
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": {
    ///         "txid": "0x0d03ad35eb8b0bb2e43e18896d22cd2a77fe54fc0b00794fb295bcf96257d0e3",
    ///         "vmstate": "HALT, BREAK",
    ///         "gas_consumed": "2.932",
    ///         "stack": [],
    ///         "notifications": [
    ///             {
    ///                 "contract": "0xac116d4b8d4ca55e6b6d4ecce2192039b51cccc5",
    ///                 "state": {
    ///                     "type": "Array",
    ///                     "value": [
    ///                         {
    ///                             "type": "ByteArray",
    ///                             "value": "7472616e73666572"
    ///                         },
    ///                         {
    ///                             "type": "ByteArray",
    ///                             "value": "45fc40a091bd0de5e5408e3dbf6b023919a6f7d9"
    ///                         },
    ///                         {
    ///                             "type": "ByteArray",
    ///                             "value": "96da23f79685e1611b99633f7a37bf07b542d42b"
    ///                         },
    ///                         {
    ///                             "type": "ByteArray",
    ///                             "value": "00345cd65804"
    ///                         }
    ///                     ]
    ///                 }
    ///             }
    ///         ]
    ///     }
    /// }
    /// </Summary>
    public class NeoGetApplicationLog : RpcRequestResponseHandler<DTOs.ApplicationLog>
    {
        public NeoGetApplicationLog(IClient client) : base(client, ApiMethods.getapplicationlog.ToString())
        {
        }

        public Task<DTOs.ApplicationLog> SendRequestAsync(string txId, object id = null)
        {
            if (txId == null) throw new ArgumentNullException(nameof(txId));
            return base.SendRequestAsync(id, txId);
        }

        public RpcRequest BuildRequest(string txId, object id = null)
        {
            if (txId == null) throw new ArgumentNullException(nameof(txId));
            return base.BuildRequest(id, txId);
        }
    }
}
