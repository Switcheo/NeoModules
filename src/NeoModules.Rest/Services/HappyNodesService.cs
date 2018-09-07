using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using NeoModules.Rest.DTOs.HappyNodes;
using NeoModules.Rest.Interfaces;

namespace NeoModules.Rest.Services
{
    public class HappyNodesService : IHappyNodesService
    {
        private static readonly string happyNodesUrl = "https://api.happynodes.f27.ventures/redis/";

        private readonly HttpClient _restClient;

        private const string UnconfirmedEndpoint = "unconfirmed";
        private const string BestBlockEndpoint = "bestblock";
        private const string LastBlockEndpoint = "lastblock";
        private const string BlockTimeEndpoint = "blocktime";
        private const string NodesEndpoint = "nodes";
        private const string NodesFlatEndpoint = "nodes_flat";
        private const string ValidatedPeersEndpoint = "validatedpeers";
        private const string EdgesEndpoint = "edges";
        private const string NodesListEndpoint = "nodeslist";

        public HappyNodesService(string customUrl = "")
        {
            _restClient = string.IsNullOrEmpty(customUrl) ? new HttpClient { BaseAddress = new Uri(happyNodesUrl) } : new HttpClient { BaseAddress = new Uri(customUrl) };
        }

        public async Task<Unconfirmed> GetUnconfirmed()
        {
            var result = await _restClient.GetAsync(UnconfirmedEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Unconfirmed.FromJson(data);
        }

        public async Task<long> GetBestBlock()
        {
            var result = await _restClient.GetAsync(BestBlockEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt64(data.Split(':')[1].Trim('}', '"'));
        }

        public async Task<long> GetLastBlock() // todo bug with '.'
        {
            var result = await _restClient.GetAsync(LastBlockEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            var test = data.Split(':')[1].Trim('}', '"', '.');
            return Convert.ToInt64(test, CultureInfo.InvariantCulture);
        }

        public async Task<decimal> GetBlockTime()
        {
            var result = await _restClient.GetAsync(BlockTimeEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToDecimal(data.Split(':')[1].Trim('}', '"'), CultureInfo.InvariantCulture);
        }

        public async Task<Nodes> GetNodes()
        {
            var result = await _restClient.GetAsync(NodesEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Nodes.FromJson(data);
        }

        public async Task<FlatNode> GetNodeById(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{NodesEndpoint}/{nodeId}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return FlatNode.FromSingleNodeJson(data);
        }

        public async Task<IList<FlatNode>> GetNodesFlat()
        {
            var result = await _restClient.GetAsync(NodesFlatEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return FlatNode.FromJson(data);
        }

        public async Task<IList<SimpleNode>> GetValidatedPeersOfNode(string nodeId)
        {
            var result = await _restClient.GetAsync($"{NodesEndpoint}/{nodeId}/{ValidatedPeersEndpoint}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return SimpleNode.FromJson(data);
        }

        public async Task<IList<EdgeNode>> GetEdges()
        {
            var result = await _restClient.GetAsync(EdgesEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return EdgeNode.FromJson(data);//TODO DTO
        }

        //public async Task<IList<Nodes>> GetNodesList()
        //{
        //    var result = await _restClient.GetAsync(NodesListEndpoint).ConfigureAwait(false);
        //    var data = await result.Content.ReadAsStringAsync();
        //    return DTOs.HappyNodes.Nodes.FromJson(data);
        //}
    }
}
