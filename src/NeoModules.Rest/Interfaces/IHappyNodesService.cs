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
        Task<decimal> GetBlockTime();
        Task<Nodes> GetNodes();
        Task<FlatNode> GetNodeById(int nodeId);
        Task<IList<FlatNode>> GetNodesFlat();
        Task<IList<SimpleNode>> GetValidatedPeersOfNode(string nodeId);
        Task<IList<EdgeNode>> GetEdges();
        //Task<IList<Nodes>> GetNodesList();
    }
}
