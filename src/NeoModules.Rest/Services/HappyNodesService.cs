using System;
using System.Net.Http;
using System.Threading.Tasks;
using NeoModules.Rest.DTOs.HappyNodes;
using NeoModules.Rest.Interfaces;

namespace NeoModules.Rest.Services
{
    public class HappyNodesService : IHappyNodesService
    {
        private static readonly string happyNodesUrl = "https://api.happynodes.f27.ventures/";

        private readonly HttpClient _restClient;

        private readonly string _unconfirmed = "unconfirmed";
        private readonly string _bestBlock = "bestblock";
        private readonly string _lastBlock = "lastblock";
        private readonly string _blockTime = "blocktime";
        private readonly string _nodes = "nodes";
        private readonly string _validatedPeers = "validatedpeers";
        private readonly string _edges = "edges";
        private readonly string _nodesList = "nodeslist";

        public HappyNodesService(string customUrl = "")
        {
            _restClient = string.IsNullOrEmpty(customUrl) ? new HttpClient { BaseAddress = new Uri(happyNodesUrl) } : new HttpClient { BaseAddress = new Uri(customUrl) };
        }

        public async Task<Unconfirmed> GetUnconfirmed()
        {
            var result = await _restClient.GetAsync(_unconfirmed);
            var data = await result.Content.ReadAsStringAsync();
            return Unconfirmed.FromJson(data);
        }

        public async Task<long> GetBestBlock()
        {
            var result = await _restClient.GetAsync(_bestBlock);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt64(data.Split(':')[1].Trim('}'));
        }

        public async Task<long> GetLastBlock()
        {
            var result = await _restClient.GetAsync(_lastBlock);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt64(data.Split(':')[1].Trim('}'));
        }

        public async Task<int> GetBlockTime()
        {
            var result = await _restClient.GetAsync(_blockTime);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt32(data.Split(':')[1].Trim('}'));
        }

        public async Task<string> GetNodes(string nodeId = "")
        {
            HttpResponseMessage result;
            if (string.IsNullOrEmpty(nodeId))
            {
                result = await _restClient.GetAsync(_nodes);
            }
            else
            {
                result = await _restClient.GetAsync($"{_nodes}/{nodeId}");
            }
            var data = await result.Content.ReadAsStringAsync();
            return data;//TODO DTO
        }

        public async Task<string> GetValidatedPeersOfNode(string nodeId)
        {
            var result = await _restClient.GetAsync($"{_nodes}/{nodeId}/{_validatedPeers}");
            var data = await result.Content.ReadAsStringAsync();
            return data;//TODO DTO
        }

        public async Task<string> GetEdges()
        {
            var result = await _restClient.GetAsync(_edges);
            var data = await result.Content.ReadAsStringAsync();
            return data;//TODO DTO
        }

        public async Task<string> GetNodesList()
        {
            var result = await _restClient.GetAsync(_nodesList);
            var data = await result.Content.ReadAsStringAsync();
            return data;//TODO DTO
        }
    }
}
