using GISServer.API.Model;
using GISServer.Domain.Model;
using NetTopologySuite.Geometries;


namespace GISServer.API.Mapper
{
    public class AspectMapper
    {
       
        public async Task<Aspect> DTOToAspect(AspectDTO aspectDTO)
        {
            Aspect aspect = new Aspect();
            aspect.Id = (Guid)aspectDTO.Id;
            aspect.Type = aspectDTO.Type;
            aspect.Code = aspectDTO.Code;
            aspect.EndPoint = aspectDTO.EndPoint;
            aspect.CommonInfo = aspectDTO.CommonInfo;
            aspect.GeographicalObjectId = aspectDTO.GeographicalObjectId;
            return aspect;
        }
        public async Task<AspectDTO> AspectToDTO(Aspect aspect)
        {
            AspectDTO aspectDTO = new AspectDTO();
            aspectDTO.Id = (Guid)aspect.Id;
            aspectDTO.Type = aspect.Type;
            aspectDTO.Code = aspect.Code;
            aspectDTO.EndPoint = aspect.EndPoint;
            aspectDTO.CommonInfo = aspect.CommonInfo;
            aspectDTO.GeographicalObjectId = aspect.GeographicalObjectId;
            return aspectDTO;
        }

    }
}
