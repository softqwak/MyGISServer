namespace GISServer.Domain.Model
{
    public interface ITopologyRepository
    {
        public Task<TopologyLink> Add(TopologyLink topologyLink);
        public Task<List<TopologyLink>> Get();
        public Task<TopologyLink> Get(Guid? id);
        public Task<(bool, string)> DeleteTopologyLink(Guid id);
    }

}