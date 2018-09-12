using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.Rest.DTOs.HappyNodes;

namespace NeoModules.Rest.Interfaces
{
    public interface IHappyNodesService
    {
        Task<Unconfirmed> GetUnconfirmed();
        Task<long> GetBestBlock();
        Task<int> GetLastBlock();
        Task<decimal> GetBlockTime();
        Task<Nodes> GetNodes();
        Task<FlatNode> GetNodeById(int nodeId);
        Task<IList<FlatNode>> GetNodesFlat();
        Task<IList<SimpleNode>> GetValidatedPeersOfNode(int nodeId);
        Task<IList<EdgeNode>> GetEdges();
        Task<IList<SimpleNode>> GetNodesList();
        Task<HistoricNetworkSize> GetDailyNodeHistory();
        Task<HistoricNetworkSize> GetWeeklyNodeHistory();
        Task<IList<NodeStability>> GetDailyNodeStability(int nodeId); 
        Task<IList<NodeStability>> GetWeeklyNodeStability(int nodeId);
        Task<IList<NodeLatency>> GetDailyNodeLatency(int nodeId);
        Task<IList<NodeLatency>> GetWeeklyNodeLatency(int nodeId);
        Task<string> GetNodeBlockheightLag(int nodeId);
        Task<EndPoints> GetEndPoints();
    }
}
