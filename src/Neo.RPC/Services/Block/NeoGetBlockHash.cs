using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Services.Block
{
    /// <Summary>
    ///     getblockhash
    ///     Returns the hash value of the corresponding block, based on the specified index.
    /// 
    ///     Parameters
    ///     Index: Block index (block height)
    /// 
    ///     Returns
    ///     string: block hash
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getblockhash","params":[10000],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "id":1,
    ///     "jsonrpc": "2.0",
    ///     "result": "4c1e879872344349067c3b1a30781eeb4f9040d3795db7922f513f6f9660b9b2"
    ///     }
    /// </Summary>
    public class NeoGetBlockHash : RpcRequestResponseHandler<string>
    {
        public NeoGetBlockHash(IClient client) : base(client, ApiMethods.getblockhash.ToString())
        {
        }

        public Task<string> SendRequestAsync(int blockIndex, object id = null)
        {
            if (blockIndex < 0) throw new ArgumentNullException(nameof(blockIndex));
            return base.SendRequestAsync(id, blockIndex);
        }

        public RpcRequest BuildRequest(int blockIndex, object id = null)
        {
            if (blockIndex < 0) throw new ArgumentNullException(nameof(blockIndex));
            return base.BuildRequest(id, blockIndex);
        }
    }
}