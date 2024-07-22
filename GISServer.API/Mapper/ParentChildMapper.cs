using GISServer.API.Model;
using GISServer.Domain.Model;
using NetTopologySuite.Geometries;


namespace GISServer.API.Mapper
{
    public class ParentChildMapper
    {
        public async Task<ParentChildObjectLink> DTOToParentChildObjectLink(ParentChildObjectLinkDTO parentChildObjectLinkDTO)
        {
            ParentChildObjectLink parentChildObjectLink = new ParentChildObjectLink();
            parentChildObjectLink.Id = (Guid)parentChildObjectLinkDTO.Id;
            parentChildObjectLink.ParentGeographicalObjectName = parentChildObjectLinkDTO.ParentGeographicalObjectName;
            parentChildObjectLink.ChildGeographicalObjectName = parentChildObjectLinkDTO.ChildGeographicalObjectName;
            parentChildObjectLink.CompletelyIncludedFlag = parentChildObjectLinkDTO.CompletelyIncludedFlag;
            parentChildObjectLink.IncludedPercent = parentChildObjectLinkDTO.IncludedPercent;
            parentChildObjectLink.CreationDateTime = parentChildObjectLinkDTO.CreationDateTime;
            parentChildObjectLink.LastUpdatedDateTime = parentChildObjectLinkDTO.LastUpdatedDateTime;
            parentChildObjectLink.ParentGeographicalObjectId = parentChildObjectLinkDTO.ParentGeographicalObjectId;
            parentChildObjectLink.ChildGeographicalObjectId = parentChildObjectLinkDTO.ChildGeographicalObjectId;
            return parentChildObjectLink;
        }
        public async Task<ParentChildObjectLinkDTO> ParentChildObjectLinkToDTO(ParentChildObjectLink parentChildObjectLink)
        {
            ParentChildObjectLinkDTO parentChildObjectLinkDTO = new ParentChildObjectLinkDTO();
            parentChildObjectLinkDTO.Id = parentChildObjectLink.Id;
            parentChildObjectLinkDTO.ParentGeographicalObjectName = parentChildObjectLink.ParentGeographicalObjectName;
            parentChildObjectLinkDTO.ChildGeographicalObjectName = parentChildObjectLink.ChildGeographicalObjectName;
            parentChildObjectLinkDTO.CompletelyIncludedFlag = parentChildObjectLink.CompletelyIncludedFlag;
            parentChildObjectLinkDTO.IncludedPercent = parentChildObjectLink.IncludedPercent;
            parentChildObjectLinkDTO.CreationDateTime = parentChildObjectLink.CreationDateTime;
            parentChildObjectLinkDTO.LastUpdatedDateTime = parentChildObjectLink.LastUpdatedDateTime;
            parentChildObjectLinkDTO.ParentGeographicalObjectId = parentChildObjectLink.ParentGeographicalObjectId;
            parentChildObjectLinkDTO.ChildGeographicalObjectId = parentChildObjectLink.ChildGeographicalObjectId;
            return parentChildObjectLinkDTO;
        }
    }
}
