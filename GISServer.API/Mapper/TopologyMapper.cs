using GISServer.API.Model;
using GISServer.Domain.Model;
using NetTopologySuite.Geometries;


namespace GISServer.API.Mapper
{
    public class TopologyMapper
    {

        public async Task<TopologyLink> DTOToTopologyLink(TopologyLinkDTO topologyLinkDTO)
        {
            TopologyLink topologyLink = new TopologyLink();
            await Task.Run(() =>
            {
                topologyLink.Id = (Guid)topologyLinkDTO.Id;
                topologyLink.Predicate = topologyLinkDTO.Predicate;
                topologyLink.LastUpdatedDateTime = topologyLinkDTO.LastUpdatedDateTime;
                topologyLink.CreationDateTime = topologyLinkDTO.CreationDateTime;
                topologyLink.CommonBorder = topologyLinkDTO.CommonBorder;
                topologyLink.GeographicalObjectInId = topologyLinkDTO.GeographicalObjectInId;
                topologyLink.GeographicalObjectOutId = topologyLinkDTO.GeographicalObjectOutId;
            });
            return topologyLink;
        }
        public async Task<TopologyLinkDTO> TopologyLinkToDTO(TopologyLink topologyLink)
        {
            TopologyLinkDTO topologyLinkDTO = new TopologyLinkDTO();
            await Task.Run(() =>
            {
                topologyLinkDTO.Id = topologyLink.Id;
                topologyLinkDTO.Predicate = topologyLink.Predicate;
                topologyLinkDTO.LastUpdatedDateTime = topologyLink.LastUpdatedDateTime;
                topologyLinkDTO.CreationDateTime = topologyLink.CreationDateTime;
                topologyLinkDTO.CommonBorder = topologyLink.CommonBorder;
                topologyLinkDTO.GeographicalObjectInId = topologyLink.GeographicalObjectInId;
                topologyLinkDTO.GeographicalObjectOutId = topologyLink.GeographicalObjectOutId;
            });
            return topologyLinkDTO;
        }


    }
}
