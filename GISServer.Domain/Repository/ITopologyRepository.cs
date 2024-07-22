namespace GISServer.Domain.Model
{
    public interface ITopologyRepository
    {
        public Task<TopologyLink> AddTopologyLink(TopologyLink topologyLink);
        public Task<List<TopologyLink>> GetTopologyLinks();
        public Task<TopologyLink> GetTopologyLink(Guid? id);
        public Task<(bool, string)> DeleteTopologyLink(Guid id);
    }

}