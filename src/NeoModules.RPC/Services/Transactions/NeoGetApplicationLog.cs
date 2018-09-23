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
    ///             "txid": "0xff488264c1abf9f5c3c17ed8071f6dd3cd809b25797a43af49316490ded8fb07",
    ///             "executions": [
    ///             {
    ///                 "trigger": "Application",
    ///                 "contract": "0x0110a8f666bcc650dc0b544e71c31491b061c79e",
    ///                 "vmstate": "HALT, BREAK",
    ///                 "gas_consumed": "2.855",
    ///                     "stack": [
    ///                     {
    ///                         "type": "Integer",
    ///                         "value": "1"
    ///                     }
    ///             ],
    ///                     "notifications": [
    ///    {
    ///                         "contract": "0xb9d7ea3062e6aeeb3e8ad9548220c4ba1361d263",
    ///                         "state": {
    ///                             "type": "Array",
    ///                             "value": [
    ///                             {
    ///                                 "type": "ByteArray",
    ///                                 "value": "7472616e73666572"
    ///                             },
    ///                             {
    ///                                 "type": "ByteArray",
    ///                                 "value": "e3069da508f128069a0cd2544b0728ccbacdfb43"
    ///                             },
    ///                             {
    ///                                 "type": "ByteArray",
    ///                                 "value": "d142f89e93b2717426a8130c37dad93aad70cff5"
    ///                             },
    ///                             {
    ///                                 "type": "ByteArray",
    ///                                 "value": "00e1f50500000000"
    ///                             }
    ///                         ]
    ///                     }
    ///                 }
    ///             ]
    ///         }
    ///     ]
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
