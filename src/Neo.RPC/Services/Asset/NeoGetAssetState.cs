using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;
using Neo.RPC.DTOs;

namespace Neo.RPC.Services.Asset
{
    /// <Summary>
    ///     getassetstate 
    ///     Queries the asset information, based on the specified asset number.
    /// 
    ///     Parameters
    ///     Asset_id: Asset ID (asset identifier), which is the transaction ID of the RegistTransaction when the asset is registered.
    ///     For NEO: c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b
    ///     For GAS: 602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7
    /// 
    ///     Example
    ///     Request
    ///     curl -X POST --data '{"jsonrpc":"2.0","method":"getassetstate","params":["c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b"],"id":1}'
    /// 
    ///     Result
    ///     {
    ///     "jsonrpc": "2.0",
    ///     "id": 1,
    ///     "result": {
    ///         "version": 0,
    ///         "id": "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b",
    ///         "type": "SystemShare",
    ///         "name": [
    ///             {
    ///                 "lang": "zh-CN",
    ///                 "name": "NEO"
    ///             },
    ///             {
    ///                 "lang": "en",
    ///                 "name": "NEO"
    ///             }
    ///         ],
    ///         "amount": "100000000",
    ///         "available": "100000000",
    ///         "precision": 0,
    ///         "owner": "00",
    ///         "admin": "Abf2qMs1pzQb8kYk9RuxtUb9jtRKJVuBJt",
    ///         "issuer": "Abf2qMs1pzQb8kYk9RuxtUb9jtRKJVuBJt",
    ///         "expiration": 2000000,
    ///         "frozen": false
    ///     }
    /// }
    /// </Summary>
    public class NeoGetAssetState : RpcRequestResponseHandler<AssetState>
    {
        public NeoGetAssetState(IClient client) : base(client, ApiMethods.getassetstate.ToString())
        {
        }

        public Task<AssetState> SendRequestAsync(string assetId, object id = null)
        {
            if (assetId == null) throw new ArgumentNullException(nameof(assetId));
            return base.SendRequestAsync(id, assetId);
        }

        public RpcRequest BuildRequest(string assetId, object id = null)
        {
            if (assetId == null) throw new ArgumentNullException(nameof(assetId));
            return base.BuildRequest(id, assetId);
        }
    }
}