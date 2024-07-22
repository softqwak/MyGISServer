
using GISServer.API.Model;
using GISServer.Domain.Model;
using NetTopologySuite.Geometries;


namespace GISServer.API.Mapper
{
    public class GeoObjectClassifiersMapper
    {
        public async Task<GeoObjectsClassifiersDTO> GOCToDTO(GeoObjectsClassifiers geoObjectsClassifiers)
        {
            return new GeoObjectsClassifiersDTO
            {
                GeoObjectId = geoObjectsClassifiers.GeoObjectId,
                ClassifierId = geoObjectsClassifiers.ClassifierId
            };
        }

        public async Task<GeoObjectsClassifiers> DTOToGOC(GeoObjectsClassifiersDTO geoObjectsClassifiersDTO)
        {
            return new GeoObjectsClassifiers
            {
                GeoObjectId = geoObjectsClassifiersDTO.GeoObjectId,
                ClassifierId = geoObjectsClassifiersDTO.ClassifierId
            };
        }
    }
}
