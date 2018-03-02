using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Services.Block
{
    /// <Summary>
    ///     getblocksysfee  
    ///     Returns the system fees of the block, based on the specified index.
    /// 
    ///     Parameters
    ///     Index：Block index
    /// 
    ///     Returns
    ///     The system fees of the block, in NeoGas units.
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getblocksysfee","params":[1005434],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": "195500"
    ///     }
    /// </Summary>
    public class NeoGetBlockSysFee : RpcRequestResponseHandler<string>
    {
        public NeoGetBlockSysFee(IClient client) : base(client, ApiMethods.getblocksysfee.ToString()) //todo
        {
        }

        public Task<string> SendRequestAsync(int blockIndex, object id = null)
        {
            if (blockIndex < 0) throw new ArgumentOutOfRangeException(nameof(blockIndex));
            return base.SendRequestAsync(id, blockIndex);
        }

        public RpcRequest BuildRequest(int blockIndex, object id = null)
        {
            if (blockIndex < 0) throw new ArgumentOutOfRangeException(nameof(blockIndex));
            return base.BuildRequest(id, blockIndex);
        }
    }
}