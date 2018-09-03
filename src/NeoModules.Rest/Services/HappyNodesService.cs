using System;
using System.Collections.Generic;
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

        private const string Unconfirmed = "unconfirmed";
        private const string BestBlock = "bestblock";
        private const string LastBlock = "lastblock";
        private const string BlockTime = "blocktime";
        private const string Nodes = "nodes";
        private const string ValidatedPeers = "validatedpeers";
        private const string Edges = "edges";
        private const string NodesList = "nodeslist";

        public HappyNodesService(string customUrl = "")
        {
            _restClient = string.IsNullOrEmpty(customUrl) ? new HttpClient { BaseAddress = new Uri(happyNodesUrl) } : new HttpClient { BaseAddress = new Uri(customUrl) };
        }

        public async Task<Unconfirmed> GetUnconfirmed()
        {
            var result = await _restClient.GetAsync(Unconfirmed).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return DTOs.HappyNodes.Unconfirmed.FromJson(data);
        }

        public async Task<long> GetBestBlock()
        {
            var result = await _restClient.GetAsync(BestBlock).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt64(data.Split(':')[1].Trim('}'));
        }

        public async Task<long> GetLastBlock()
        {
            var result = await _restClient.GetAsync(LastBlock).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt64(data.Split(':')[1].Trim('}'));
        }

        public async Task<int> GetBlockTime()
        {
            var result = await _restClient.GetAsync(BlockTime).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return Convert.ToInt32(data.Split(':')[1].Trim('}'));
        }

        public async Task<string> GetNodes(string nodeId = "")
        {
            HttpResponseMessage result;
            if (string.IsNullOrEmpty(nodeId))
            {
                result = await _restClient.GetAsync(Nodes).ConfigureAwait(false);
            }
            else
            {
                result = await _restClient.GetAsync($"{Nodes}/{nodeId}").ConfigureAwait(false);
            }
            var data = await result.Content.ReadAsStringAsync();
            return data;//TODO DTO
        }

        public async Task<string> GetValidatedPeersOfNode(string nodeId)
        {
            var result = await _restClient.GetAsync($"{Nodes}/{nodeId}/{ValidatedPeers}").ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return data;//TODO DTO
        }

        public async Task<string> GetEdges()
        {
            var result = await _restClient.GetAsync(Edges).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return data;//TODO DTO
        }

        public async Task<IList<Nodes>> GetNodesList()
        {
            var result = await _restClient.GetAsync(NodesList).ConfigureAwait(false);
            var data = await result.Content.ReadAsStringAsync();
            return DTOs.HappyNodes.Nodes.FromJson(data);
        }
    }
}
