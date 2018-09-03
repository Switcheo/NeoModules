using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.Rest.DTOs.HappyNodes;

namespace NeoModules.Rest.Interfaces
{
    public interface IHappyNodesService
    {
        Task<Unconfirmed> GetUnconfirmed();
        Task<long> GetBestBlock();
        Task<long> GetLastBlock();
        Task<int> GetBlockTime();
        Task<string> GetNodes(string nodeId = "");
        Task<string> GetValidatedPeersOfNode(string nodeId);
        Task<string> GetEdges();
        Task<IList<Nodes>> GetNodesList();
    }
}
