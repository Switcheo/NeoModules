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
        private const string UnconfirmedEndpoint = "unconfirmed";
        private const string BestBlockEndpoint = "bestblock";
        private const string LastBlockEndpoint = "lastblock";
        private const string BlockTimeEndpoint = "blocktime";
        private const string NodesEndpoint = "nodes";
        private const string NodesFlatEndpoint = "nodes_flat";
        private const string ValidatedPeersEndpoint = "validatedpeers";
        private const string EdgesEndpoint = "edges";
        private const string NodesListEndpoint = "nodeslist";
        private const string DailyNetworkSizeEndpoint = "historic/network/size/daily";
        private const string WeeklyNetworkSizeEndpoint = "historic/network/size/weekly";
        private const string DailyNodeStabilityEndpoint = "historic/node/stability/daily";
        private const string WeeklyNodeStabilityEndpoint = "historic/node/stability/weekly";
        private const string DailyNodeLatencyEndpoint = "historic/node/latency/daily";
        private const string WeeklyNodeLatencyEndpoint = "historic/node/latency/weekly";
        private const string NodeBlockheightLagEndpoint = " historic/node/blockheightlag";
        private const string Endpoints = "endpoints";
        private static readonly string happyNodesUrl = "https://api.happynodes.f27.ventures/redis/";


        private readonly HttpClient _restClient;

        public HappyNodesService(string customUrl = "")
        {
            _restClient = string.IsNullOrEmpty(customUrl)
                ? new HttpClient { BaseAddress = new Uri(happyNodesUrl) }
                : new HttpClient { BaseAddress = new Uri(customUrl) };
        }

        //https://api.happynodes.f27.ventures/redis/bestblock
        public async Task<long> GetBestBlock()
        {
            var result = await _restClient.GetAsync(BestBlockEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt64(data.Split(':')[1].Trim('}', '"'));
        }

        //https://api.happynodes.f27.ventures/redis/lastblock
        public async Task<int> GetLastBlock() // todo check with creator about decimals in lastblock
        {
            var result = await _restClient.GetAsync(LastBlockEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt32(Convert.ToDecimal(data.Split(':')[1].Trim('}', '"'), CultureInfo.InvariantCulture));
        }

        //https://api.happynodes.f27.ventures/redis/blocktime
        public async Task<decimal> GetBlockTime()
        {
            var result = await _restClient.GetAsync(BlockTimeEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToDecimal(data.Split(':')[1].Trim('}', '"'), CultureInfo.InvariantCulture);
        }

        //https://api.happynodes.f27.ventures/redis/unconfirmed
        public async Task<Unconfirmed> GetUnconfirmed()
        {
            var result = await _restClient.GetAsync(UnconfirmedEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Unconfirmed.FromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/nodes_flat
        public async Task<IList<FlatNode>> GetNodesFlat()
        {
            var result = await _restClient.GetAsync(NodesFlatEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return FlatNode.FromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/nodes
        public async Task<Nodes> GetNodes()
        {
            var result = await _restClient.GetAsync(NodesEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Nodes.FromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/nodes/:node_id
        public async Task<FlatNode> GetNodeById(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{NodesEndpoint}/{nodeId}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return FlatNode.FromSingleNodeJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/nodes/:node_id/validatedpeers
        public async Task<IList<SimpleNode>> GetValidatedPeersOfNode(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{NodesEndpoint}/{nodeId}/{ValidatedPeersEndpoint}")
                .ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return SimpleNode.FromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/edges
        public async Task<IList<EdgeNode>> GetEdges()
        {
            var result = await _restClient.GetAsync(EdgesEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return EdgeNode.FromJson(data); //TODO DTO
        }

        //https://api.happynodes.f27.ventures/redis/nodeslist
        public async Task<IList<SimpleNode>> GetNodesList()
        {
            var result = await _restClient.GetAsync(NodesListEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return SimpleNode.FromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/historic/network/size/daily
        public async Task<HistoricNetworkSize> GetDailyNodeHistory()
        {
            var result = await _restClient.GetAsync(DailyNetworkSizeEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return HistoricNetworkSize.FromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/historic/network/size/weekly
        public async Task<HistoricNetworkSize> GetWeeklyNodeHistory()
        {
            var result = await _restClient.GetAsync(WeeklyNetworkSizeEndpoint).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return HistoricNetworkSize.FromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/historic/node/stability/daily/:node_id
        public async Task<IList<NodeStability>> GetDailyNodeStability(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{DailyNodeStabilityEndpoint}/{nodeId}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return NodeStability.DailyFromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/historic/node/stability/weekly/:node_id
        public async Task<IList<NodeStability>> GetWeeklyNodeStability(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{WeeklyNodeStabilityEndpoint}/{nodeId}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return NodeStability.WeeklyFromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/historic/node/latency/daily/:node_id
        public async Task<IList<NodeLatency>> GetDailyNodeLatency(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{DailyNodeLatencyEndpoint}/{nodeId}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return NodeLatency.DailyFromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/historic/node/latency/weekly/:node_id
        public async Task<IList<NodeLatency>> GetWeeklyNodeLatency(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{WeeklyNodeLatencyEndpoint}/{nodeId}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return NodeLatency.WeeklyFromJson(data);
        }

        //https://api.happynodes.f27.ventures/redis/historic/node/blockheightlag/:node_id
        public async Task<string> GetNodeBlockheightLag(int nodeId)
        {
            if (nodeId < 0) throw new ArgumentOutOfRangeException(nameof(nodeId));
            var result = await _restClient.GetAsync($"{NodeBlockheightLagEndpoint}/{nodeId}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return data; //TODO dto
        }

        //https://api.happynodes.f27.ventures/redis/endpoints
        public async Task<EndPoints> GetEndPoints()
        {
            var result = await _restClient.GetAsync(Endpoints).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return EndPoints.FromJson(data);
        }
    }
}