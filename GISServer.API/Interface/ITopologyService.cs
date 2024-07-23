using GISServer.API.Model;
using GISServer.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace GISServer.API.Interface
{
    public interface ITopologyService
    {
        public TopologyLinkDTO CreateGuid(TopologyLinkDTO topologyLinkDTO);
        public Task<TopologyLinkDTO> AddTopologyLink(TopologyLinkDTO topologyLinkDTO);
        public Task<List<TopologyLinkDTO>> GetTopologyLinks();
        public Task<(bool, string)> DeleteTopologyLink(Guid id);
        public Task<TopologyLinkDTO> GetCommonBorder(TopologyLinkDTO topologyLinkDTO);

    }
}
